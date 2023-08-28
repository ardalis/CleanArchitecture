using Ardalis.HttpClientTestExtensions;
using FluentAssertions;
using NimblePros.SampleToDo.Web;
using NimblePros.SampleToDo.Web.Contributors;
using Xunit;

namespace NimblePros.SampleToDo.FunctionalTests.Contributors;

[Collection("Sequential")]
public class ContributorUpdate : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ContributorUpdate(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task UpdatesContributorNameAndReturnsUpdatedRecord()
  {
    var newName = Guid.NewGuid().ToString();
    var request = new UpdateContributorRequest() { Id = SeedData.Contributor1.Id, Name = newName };
    var putRoute = UpdateContributorRequest.BuildRoute(SeedData.Contributor1.Id);

    var content = StringContentHelpers.FromModelAsJson(request);

    var result = await _client.PutAndDeserializeAsync<UpdateContributorResponse>(putRoute, content);

    result.Contributor.Name.Should().Be(newName);
    result.Contributor.Id.Should().BeGreaterThan(0);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenMissingContributorId()
  {
    int invalidId = 1000;
    var putRoute = UpdateContributorRequest.BuildRoute(invalidId);
    var request = new UpdateContributorRequest() { Id = invalidId, Name = "some name" };
    var content = StringContentHelpers.FromModelAsJson(request);

    _ = await _client.PutAndEnsureNotFoundAsync(putRoute, content);
  }

  [Fact]
  public async Task ReturnsBadRequestIfRouteIdDoesNotMatchBodyId()
  {
    int routeId = 1;
    int bodyId = 2;
    var putRoute = UpdateContributorRequest.BuildRoute(routeId);
    var request = new UpdateContributorRequest() { Id = bodyId, Name = "some name" };
    var content = StringContentHelpers.FromModelAsJson(request);

    _ = await _client.PutAndEnsureBadRequestAsync(putRoute, content);
  }

}
