using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Web.Contributors;

namespace Clean.Architecture.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ContributorList(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client = factory.CreateClient();

  [Fact]
  public async Task ReturnsTwoContributors()
  {
    var result = await _client.GetAndDeserializeAsync<ContributorListResponse>("/Contributors");

    result.TotalCount.ShouldBe(SeedData.NUMBER_OF_CONTRIBUTORS);
    result.Items.ShouldContain(i => i.Name == SeedData.Contributor1Name.Value);
    result.Items.ShouldContain(i => i.Name == SeedData.Contributor2Name.Value);
  }
}
