using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.IntegrationTests.Data;

public class EfRepositoryDelete : BaseEfRepoTestFixture
{
  [Fact]
  public async Task DeletesItemAfterAddingIt()
  {
    var cancellationToken = TestContext.Current.CancellationToken;
    // add a project
    var repository = GetRepository();
    var initialName = Guid.NewGuid().ToString();
    var project = new Project(ProjectName.From(initialName));
    await repository.AddAsync(project, cancellationToken);

    // delete the item
    await repository.DeleteAsync(project, cancellationToken);

    // verify it's no longer there
    Assert.DoesNotContain(await repository.ListAsync(cancellationToken),
        project => project.Name == initialName);
  }
}
