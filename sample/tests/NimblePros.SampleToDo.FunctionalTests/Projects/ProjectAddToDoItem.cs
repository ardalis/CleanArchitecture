using Ardalis.HttpClientTestExtensions;
using Xunit;
using FluentAssertions;
using NimblePros.SampleToDo.Web;
using NimblePros.SampleToDo.Web.Projects;
using NimblePros.SampleToDo.Web.Endpoints.Projects;

namespace NimblePros.SampleToDo.FunctionalTests.Projects;

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
    var toDoTitle = Guid.NewGuid().ToString();
    var testProjectId = SeedData.TestProject1.Id;
    var request = new CreateToDoItemRequest()
    {
      Title = toDoTitle,
      ProjectId = testProjectId,
      Description = toDoTitle
    };
    var content = StringContentHelpers.FromModelAsJson(request);

    var result = await _client.PostAsync(CreateToDoItemRequest.BuildRoute(testProjectId), content);

    // useful for debugging error responses:
    var stringContent = await result.Content.ReadAsStringAsync();

    var expectedRoute = GetProjectByIdRequest.BuildRoute(testProjectId);

    // TODO: Figure out why FastEndpoints isn't setting Location header
    result.Headers.Location!.ToString().Should().Be(expectedRoute);

    var updatedProject = await _client.GetAndDeserializeAsync<GetProjectByIdResponse>(expectedRoute);
    updatedProject.Items.Should().ContainSingle(item => item.Title == toDoTitle);
  }
}
