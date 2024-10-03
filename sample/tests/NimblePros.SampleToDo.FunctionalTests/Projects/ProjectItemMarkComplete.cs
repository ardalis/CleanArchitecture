using System.Text;
using Newtonsoft.Json;
using Xunit;
using NimblePros.SampleToDo.Web.ProjectEndpoints;
using Ardalis.HttpClientTestExtensions;
using NimblePros.SampleToDo.Web.Endpoints.Projects;
using NimblePros.SampleToDo.Web.Projects;
using FluentAssertions;
using NimblePros.SampleToDo.FunctionalTests.ClassFixtures;

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

  [Fact]
  public async Task MarksIncompleteItemComplete()
  {
    var projectId = 1;
    var itemId = 1;

    var jsonContent = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

    var route = MarkItemCompleteRequest.BuildRoute(projectId, itemId);
    var response = await _client.PostAsync(route, jsonContent);
    response.EnsureSuccessStatusCode();

    var stringResponse = await response.Content.ReadAsStringAsync();
    Assert.Equal("", stringResponse);

    // confirm item is complete
    var project = await _client.GetAndDeserializeAsync<GetProjectByIdResponse>(GetProjectByIdRequest.BuildRoute(projectId));
    project.Items.First(i => i.Id == itemId).IsDone.Should().BeTrue();
  }
}
