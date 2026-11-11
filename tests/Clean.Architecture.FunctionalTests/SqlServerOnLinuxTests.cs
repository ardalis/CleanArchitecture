using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Web.Contributors;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;

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
