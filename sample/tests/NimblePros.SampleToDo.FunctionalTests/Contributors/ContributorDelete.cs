using Ardalis.HttpClientTestExtensions;
using NimblePros.SampleToDo.Web;
using NimblePros.SampleToDo.Web.Contributors;
using Xunit;

namespace NimblePros.SampleToDo.FunctionalTests.Contributors;

[Collection("Sequential")]
public class ContributorDelete : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ContributorDelete(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task DeletesExistingContributor()
  {
    var deleteRoute = DeleteContributorRequest.BuildRoute(SeedData.Contributor1.Id);
    _ = await _client.DeleteAndEnsureNoContentAsync(deleteRoute);

    string getRoute = GetContributorByIdRequest.BuildRoute(SeedData.Contributor1.Id);
    _ = await _client.GetAndEnsureNotFoundAsync(getRoute);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenMissingContributorId()
  {
    int invalidId = 1000;
    var deleteRoute = DeleteContributorRequest.BuildRoute(invalidId);
    _ = await _client.DeleteAndEnsureNotFoundAsync(deleteRoute);
  }
}
