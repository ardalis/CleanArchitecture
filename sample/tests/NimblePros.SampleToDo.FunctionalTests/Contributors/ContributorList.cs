using NimblePros.SampleToDo.Web;
using NimblePros.SampleToDo.Web.Contributors;

namespace NimblePros.SampleToDo.FunctionalTests.Contributors;

[Collection("Sequential")]
public class ContributorList : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ContributorList(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsTwoContributors()
  {
    var result = await _client.GetAndDeserializeAsync<ContributorListResponse>("/Contributors");

    Assert.Equal(25, result.TotalCount);
    Assert.Contains(result.Items, i => i.Name == "Fake 1");
    Assert.Contains(result.Items, i => i.Name == "Fake 2");
  }
}
