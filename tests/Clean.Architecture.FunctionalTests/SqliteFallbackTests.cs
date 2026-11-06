using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Web.Contributors;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.FunctionalTests;

public class SqliteFallbackTests
{
  [Fact]
  public async Task AutoProvider_FallsBackToSqlite_WhenDockerIsUnavailable()
  {
    await using var factory =
      CustomWebApplicationFactory<Program>.CreateWithContainerStartFailure(
        TestDatabaseProvider.Auto,
        new DotNet.Testcontainers.Builders.DockerUnavailableException("Simulated Docker outage"));

    await factory.InitializeAsync();
    using var client = factory.CreateClient();
    var result = await client.GetAndDeserializeAsync<ContributorRecord>(
      GetContributorByIdRequest.BuildRoute(1));

    factory.ActiveDatabaseProvider.ShouldBe(TestDatabaseProvider.Sqlite);
    result.Id.ShouldBe(1);
    result.Name.ShouldBe(SeedData.Contributor1Name.Value);
  }

  [Fact]
  public async Task ExplicitSqlServer_DoesNotFallback_WhenDockerIsUnavailable()
  {
    await using var factory =
      CustomWebApplicationFactory<Program>.CreateWithContainerStartFailure(
        TestDatabaseProvider.SqlServer,
        new DotNet.Testcontainers.Builders.DockerUnavailableException("Simulated Docker outage"));

    await Should.ThrowAsync<DotNet.Testcontainers.Builders.DockerUnavailableException>(
      async () => await factory.InitializeAsync());
  }

  [Fact]
  public async Task AutoProvider_PropagatesUnexpectedContainerStartFailure()
  {
    await using var factory =
      CustomWebApplicationFactory<Program>.CreateWithContainerStartFailure(
        TestDatabaseProvider.Auto,
        new InvalidOperationException("Simulated unexpected startup failure"));

    await Should.ThrowAsync<InvalidOperationException>(
      async () => await factory.InitializeAsync());
    factory.ActiveDatabaseProvider.ShouldBe(TestDatabaseProvider.Auto);
  }

  [Fact]
  public async Task UsesSqlite_WhenSqliteProviderIsSelected()
  {
    await using var factory =
      CustomWebApplicationFactory<Program>.CreateWithProvider(
        TestDatabaseProvider.Sqlite);

    using var scope = factory.Services.CreateScope();

    var dbContext =
      scope.ServiceProvider.GetRequiredService<AppDbContext>();

    factory.ActiveDatabaseProvider
      .ShouldBe(TestDatabaseProvider.Sqlite);
    dbContext.Database.IsSqlite().ShouldBeTrue();
  }

  [Fact]
  public async Task ContributorEndpoint_WorksWithSqlite()
  {
    await using var factory =
      CustomWebApplicationFactory<Program>.CreateWithProvider(
        TestDatabaseProvider.Sqlite);

    using var client = factory.CreateClient();

    var result =
      await client.GetAndDeserializeAsync<ContributorRecord>(
        GetContributorByIdRequest.BuildRoute(1));

    factory.ActiveDatabaseProvider
      .ShouldBe(TestDatabaseProvider.Sqlite);
    result.Id.ShouldBe(1);
    result.Name.ShouldBe(SeedData.Contributor1Name.Value);
  }
}
