using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
  [Fact]
  public async Task AddsProjectAndSetsId()
  {
    var testProjectName = ProjectName.From("testProject");
    var repository = GetRepository();
    var project = new Project(testProjectName);

    var item = new ToDoItem();
    item.Title = "test item title";
    project.AddItem(item);

    await repository.AddAsync(project);

    var newProject = (await repository.ListAsync())
                    .FirstOrDefault();

    Assert.Equal(testProjectName, newProject?.Name);
    Assert.True(newProject?.Id.Value > 0);
  }
}
