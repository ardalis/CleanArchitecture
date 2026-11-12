using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Web.Contributors;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Testcontainers.MsSql;

namespace Clean.Architecture.FunctionalTests;

public class SqlServerOnLinuxTests
{
  // CI sets this so the SQL Server container check can never be silently skipped.
  private const string RequireSqlServerVariable = "REQUIRE_SQL_SERVER_TESTS";

  private static bool IsSqlServerRequired() =>
    string.Equals(
      Environment.GetEnvironmentVariable(RequireSqlServerVariable),
      "true",
      StringComparison.OrdinalIgnoreCase);

  // Functional tests build the schema with EnsureCreated, so this is the only test that
  // runs the real EF Core migrations against SQL Server (see issue #1194).
  [Fact]
  public async Task Migrations_ApplyToEmptyDatabase_AndPreserveExistingContributors()
  {
    const string MigrationBeforeDbGeneratedIds = "20251113164108_UpdateForNet10";
    var cancellationToken = TestContext.Current.CancellationToken;

    MsSqlContainer container;
    try
    {
      await DockerEnvironment.EnsureLinuxContainersAvailableAsync(cancellationToken);
      container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-latest")
        .WithPassword("Your_password123!")
        .Build();
      await container.StartAsync(cancellationToken);
    }
    catch (DockerUnavailableException ex) when (!IsSqlServerRequired())
    {
      Assert.Skip(
        "Docker is unavailable, so the SQL Server migration test was skipped. " +
        $"Set {RequireSqlServerVariable}=true to make this a failure. Error: {ex.Message}");
      return;
    }

    await using (container)
    {
      var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlServer(container.GetConnectionString())
        .Options;

      await using var db = new AppDbContext(options);
      var migrator = db.GetService<IMigrator>();

      // Simulate an existing database created before the switch to DB-generated IDs.
      await migrator.MigrateAsync(MigrationBeforeDbGeneratedIds, cancellationToken);
      await db.Database.ExecuteSqlAsync(
        $"INSERT INTO [Contributors] ([Id], [Name], [Status]) VALUES (42, {"Existing"}, 0)",
        cancellationToken);

      // Upgrade to the latest migration.
      await migrator.MigrateAsync(cancellationToken: cancellationToken);
      (await db.Database.GetPendingMigrationsAsync(cancellationToken)).ShouldBeEmpty();

      // Existing rows survive and new rows get an IDENTITY-generated Id.
      var existing = await db.Database
        .SqlQuery<string>($"SELECT [Name] AS [Value] FROM [Contributors] WHERE [Id] = 42")
        .SingleAsync(cancellationToken);
      existing.ShouldBe("Existing");

      await db.Database.ExecuteSqlAsync(
        $"INSERT INTO [Contributors] ([Name], [Status]) VALUES ({"New"}, 0)",
        cancellationToken);
      var newId = await db.Database
        .SqlQuery<int>($"SELECT [Id] AS [Value] FROM [Contributors] WHERE [Name] = {"New"}")
        .SingleAsync(cancellationToken);
      newId.ShouldBeGreaterThan(42);

      // The Down migration must also run cleanly.
      await migrator.MigrateAsync(MigrationBeforeDbGeneratedIds, cancellationToken);
      var namesAfterDown = await db.Database
        .SqlQuery<string>($"SELECT [Name] AS [Value] FROM [Contributors]")
        .ToListAsync(cancellationToken);
      namesAfterDown.ShouldBe(["Existing", "New"], ignoreOrder: true);
    }
  }

  [Fact]
  public async Task SqlServerContainer_StartsAndServesContributorEndpoint()
  {
    await using var factory =
      CustomWebApplicationFactory<Program>.CreateWithProvider(
        TestDatabaseProvider.SqlServer);

    try
    {
      await factory.InitializeAsync();
    }
    catch (DockerUnavailableException ex) when (!IsSqlServerRequired())
    {
      Assert.Skip(
        "Docker is unavailable, so the SQL Server container test was skipped. " +
        $"Set {RequireSqlServerVariable}=true to make this a failure. Error: {ex.Message}");
    }

    using var client = factory.CreateClient();
    using var scope = factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    factory.ActiveDatabaseProvider.ShouldBe(TestDatabaseProvider.SqlServer);
    db.Database.IsSqlServer().ShouldBeTrue();

    var contributor = await client.GetAndDeserializeAsync<ContributorRecord>(
      GetContributorByIdRequest.BuildRoute(1));

    contributor.Id.ShouldBe(1);
    contributor.Name.ShouldBe(SeedData.Contributor1Name.Value);
  }
}
