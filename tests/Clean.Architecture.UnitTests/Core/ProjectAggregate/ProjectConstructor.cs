using Clean.Architecture.Core.ProjectAggregate;
using Xunit;

namespace Clean.Architecture.UnitTests.Core.ProjectAggregate;

public class ProjectConstructor
{
  private string _testName = "test name";
  private PriorityStatus _testPriority = PriorityStatus.Backlog;
  private Project? _testProject;

  private Project CreateProject()
  {
    return new Project(_testName, _testPriority);
  }

  [Fact]
  public void InitializesName()
  {
    _testProject = CreateProject();

    Assert.Equal(_testName, _testProject.Name);
  }

  [Fact]
  public void InitializesPriority()
  {
    _testProject = CreateProject();

    Assert.Equal(_testPriority, _testProject.Priority);
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
}
