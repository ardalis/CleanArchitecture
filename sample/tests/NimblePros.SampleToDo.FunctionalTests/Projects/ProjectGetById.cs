using Ardalis.HttpClientTestExtensions;
using NimblePros.SampleToDo.Web;
using NimblePros.SampleToDo.Web.Endpoints.Projects;
using NimblePros.SampleToDo.Web.Projects;
using Xunit;

namespace NimblePros.SampleToDo.FunctionalTests.Projects;

[Collection("Sequential")]
public class ProjectGetById : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ProjectGetById(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsSeedProjectGivenId1()
  {
    var result = await _client.GetAndDeserializeAsync<GetProjectByIdResponse>(GetProjectByIdRequest.BuildRoute(1));

    Assert.Equal(1, result.Id);
    Assert.Equal(SeedData.TestProject1.Name, result.Name);
    Assert.Equal(3, result.Items.Count);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId0()
  {
    var route = GetProjectByIdRequest.BuildRoute(0);
    _ = await _client.GetAndEnsureNotFoundAsync(route);
  }
}
