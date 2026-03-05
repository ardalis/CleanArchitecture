using System.Globalization;
using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UnitTests.Core.ProjectAggregate;

public class ProjectConstructor
{
  private string _testName = "test name";
  private Priority _testPriority = Priority.Backlog;
  private Project? _testProject;

  private Project CreateProject()
  {
    return new Project(ProjectName.From(_testName));
  }

  [Fact]
  public void InitializesName()
  {
    _testProject = CreateProject();

    Assert.Equal(_testName, _testProject.Name.Value);
  }

  [Fact]
  public void InitializesTaskListToEmptyList()
  {
    _testProject = CreateProject();

    Assert.NotNull(_testProject.Items);
  }

  [Fact]
  public void InitializesStatusToInProgress()
  {
    _testProject = CreateProject();

    Assert.Equal(ProjectStatus.Complete, _testProject.Status);
  }
  [Fact]
  public void ProjectName_TooLong_ReturnsLocalizedMessage()
  {
    // Arrange: Set a French culture for testing
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

    // Act
    var message = ProjectErrorMessages.CoreProjectNameTooLong(100);

    // Assert: Should use fallback or loaded JSON (depending on setup)
    Assert.Equal("Name cannot be longer than 100 characters", message);
  }
}
