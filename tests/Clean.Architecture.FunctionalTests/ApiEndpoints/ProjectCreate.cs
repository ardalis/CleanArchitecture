using Ardalis.HttpClientTestExtensions;
using Clean.Architecture.Web.Endpoints.ProjectEndpoints;
using Xunit;
using FluentAssertions;
using Clean.Architecture.Web;

namespace Clean.Architecture.FunctionalTests.ApiEndpoints;

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
    string testName = Guid.NewGuid().ToString();
    var request = new CreateProjectRequest() { Name = testName };
    var content = StringContentHelpers.FromModelAsJson(request);

    var result = await _client.PostAndDeserializeAsync<CreateProjectResponse>(
      CreateProjectRequest.Route, content);

    result.Name.Should().Be(testName);
    result.Id.Should().BeGreaterThan(0);
  }
}

[Collection("Sequential")]
public class ProjectAddToDoItem : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ProjectAddToDoItem(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task AddsItemAndReturnsRouteToProject()
  {
    string toDoTitle = Guid.NewGuid().ToString();
    int testProjectId = SeedData.TestProject1.Id;
    var request = new CreateToDoItemRequest() { 
      Title = toDoTitle, 
      ProjectId = testProjectId,
      Description = toDoTitle
    };
    var content = StringContentHelpers.FromModelAsJson(request);

    var result = await _client.PostAsync(CreateToDoItemRequest.BuildRoute(testProjectId), content);
    
    // useful for debugging error responses:
    // var stringContent = await result.Content.ReadAsStringAsync();

    string expectedRoute = GetProjectByIdRequest.BuildRoute(testProjectId);
    result.Headers.Location!.ToString().Should().Be(expectedRoute);

    var updatedProject = await _client.GetAndDeserializeAsync<GetProjectByIdResponse>(expectedRoute);

    updatedProject.Items.Should().ContainSingle(item => item.Title == toDoTitle);
  }
}
