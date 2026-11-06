using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Web.Contributors;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.FunctionalTests;

public class SqlServerOnLinuxTests
{
  [Fact]
  public async Task SqlServerContainer_StartsAndServesContributorEndpoint()
  {
    await using var factory =
      CustomWebApplicationFactory<Program>.CreateWithProvider(
        TestDatabaseProvider.SqlServer);

    await factory.InitializeAsync();
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
