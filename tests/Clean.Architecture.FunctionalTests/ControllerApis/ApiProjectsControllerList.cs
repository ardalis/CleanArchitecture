using System.Text;
using Ardalis.HttpClientTestExtensions;
using Clean.Architecture.Web;
using Clean.Architecture.Web.ApiModels;
using Newtonsoft.Json;
using Xunit;

namespace Clean.Architecture.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ProjectCreate : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ProjectCreate(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneProject()
  {
    var result = await _client.GetAndDeserializeAsync<IEnumerable<ProjectDTO>>("/api/projects");

    Assert.Single(result);
    Assert.Contains(result, i => i.Name == SeedData.TestProject1.Name);
  }

  [Fact]
  public async Task CreateProject()
  {
    string projectName = "Test Project 2";
    var result = await _client.PostAndDeserializeAsync<ProjectDTO>("/api/projects", new StringContent(JsonConvert.SerializeObject(new CreateProjectDTO(projectName)), Encoding.UTF8, "application/json"));
    Assert.NotNull(result);
    Assert.Equal(projectName, result.Name);
  }
}
