using NimblePros.SampleToDo.Web;
using NimblePros.SampleToDo.Web.Endpoints.Projects;
using NimblePros.SampleToDo.Web.Projects;
using Shouldly;

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

    result.Id.ShouldBe(1);
    result.Name.ShouldBe(SeedData.TestProject1.Name.Value);
    result.Items.Count.ShouldBe(3);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId0()
  {
    var route = GetProjectByIdRequest.BuildRoute(0);
    _ = await _client.GetAndEnsureNotFoundAsync(route);
  }
}
