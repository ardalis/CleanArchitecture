using NimblePros.SampleToDo.Core.ProjectAggregate;
using Xunit;

namespace NimblePros.SampleToDo.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
  [Fact]
  public async Task AddsProjectAndSetsId()
  {
    var testProjectName = "testProject";
    var testProjectPriority = Priority.Backlog;
    var repository = GetRepository();
    var project = new Project(testProjectName, testProjectPriority);

    await repository.AddAsync(project);

    var newProject = (await repository.ListAsync())
                    .FirstOrDefault();

    Assert.Equal(testProjectName, newProject?.Name);
    Assert.Equal(testProjectPriority, newProject?.Priority);
    Assert.True(newProject?.Id > 0);
  }
}
