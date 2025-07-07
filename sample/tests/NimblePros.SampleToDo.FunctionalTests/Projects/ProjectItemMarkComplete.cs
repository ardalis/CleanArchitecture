using NimblePros.SampleToDo.FunctionalTests.ClassFixtures;
using NimblePros.SampleToDo.Web.Endpoints.Projects;
using NimblePros.SampleToDo.Web.ProjectEndpoints;
using NimblePros.SampleToDo.Web.Projects;
using Shouldly;

namespace NimblePros.SampleToDo.FunctionalTests.Projects;

[Collection("Sequential")]
public class ProjectItemMarkComplete : 
  IClassFixture<CustomWebApplicationFactory<Program>>,
  IClassFixture<SmtpServerFixture>
{
  private readonly HttpClient _client;

  public ProjectItemMarkComplete(CustomWebApplicationFactory<Program> factory, SmtpServerFixture smtpServer)
  {
    _client = factory.CreateClient();
    smtpServer.EnsureContainerIsRunning();
  }

  /// <summary>
  /// Currently this fails if you don't have a local mailserver running, like papercut
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task MarksIncompleteItemComplete()
  {
    // TODO: Arrange to use a fake mail server for this test
    var projectId = 1;
    var itemId = 1;

    var jsonContent = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

    var route = MarkItemCompleteRequest.BuildRoute(projectId, itemId);
    var response = await _client.PostAsync(route, jsonContent);
    response.EnsureSuccessStatusCode();

    var stringResponse = await response.Content.ReadAsStringAsync();
    stringResponse.ShouldBeEmpty();

    // confirm item is complete
    var project = await _client.GetAndDeserializeAsync<GetProjectByIdResponse>(GetProjectByIdRequest.BuildRoute(projectId));
    project.Items.First(i => i.Id == itemId).IsDone.ShouldBeTrue();
  }
}
