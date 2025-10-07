using NimblePros.SampleToDo.Web;
using NimblePros.SampleToDo.Web.Projects;

namespace NimblePros.SampleToDo.FunctionalTests.Projects;

/// <summary>
/// Fluent builder for CreateToDoItemRequest to make tests more readable and maintainable
/// </summary>
public class CreateToDoItemRequestBuilder
{
  private readonly CreateToDoItemRequest _request = new();

  public CreateToDoItemRequestBuilder WithProjectId(int projectId)
  {
    _request.ProjectId = projectId;
    return this;
  }

  public CreateToDoItemRequestBuilder WithTitle(string title)
  {
    _request.Title = title;
    return this;
  }

  public CreateToDoItemRequestBuilder WithDescription(string description)
  {
    _request.Description = description;
    return this;
  }

  public CreateToDoItemRequestBuilder WithContributorId(int? contributorId)
  {
    _request.ContributorId = contributorId;
    return this;
  }

  public CreateToDoItemRequestBuilder WithValidDefaults()
  {
    var uniqueId = Guid.NewGuid().ToString()[..8];
    var timestamp = DateTimeOffset.UtcNow.Ticks.ToString()[^8..]; // Last 8 digits of ticks
    return WithProjectId(SeedData.TestProject1.Id.Value)
           .WithTitle($"Test Todo {uniqueId}-{timestamp}")
           .WithDescription($"Test Description {uniqueId}-{timestamp}");
  }

  public CreateToDoItemRequest Build() => _request;

  public static CreateToDoItemRequestBuilder Create() => new();
}
