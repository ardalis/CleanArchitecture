using NimblePros.SampleToDo.Web.Contributors;
using Shouldly;

namespace NimblePros.SampleToDo.FunctionalTests.Contributors;

[Collection("Sequential")]
public class ContributorCreate : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ContributorCreate(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneContributor()
  {
    var testName = Guid.NewGuid().ToString();
    var request = new CreateContributorRequest() { Name = testName };
    var content = StringContentHelpers.FromModelAsJson(request);

    var result = await _client.PostAndDeserializeAsync<CreateContributorResponse>(
        CreateContributorRequest.Route, content);

    result.Name.ShouldBe(testName);
    result.Id.ShouldBeGreaterThan(0);
  }
}
