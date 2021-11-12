using Ardalis.HttpClientTestExtensions;
using Clean.Architecture.Web;
using Clean.Architecture.Web.Endpoints.ProjectEndpoints;
using Xunit;

namespace Clean.Architecture.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ProjectList : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient _client;

  public ProjectList(CustomWebApplicationFactory<WebMarker> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneProject()
  {
    var result = await _client.GetAndDeserialize<ProjectListResponse>("/Projects");

    Assert.Single(result.Projects);
    Assert.Contains(result.Projects, i => i.Name == SeedData.TestProject1.Name);
  }
}
