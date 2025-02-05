using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.IntegrationTests.Data;

public class EfRepositoryUpdate : BaseEfRepoTestFixture
{
  [Fact]
  public async Task UpdatesItemAfterAddingIt()
  {
    // add a Contributor
    var repository = GetRepository();
    var initialName = Guid.NewGuid().ToString();
    var Contributor = new Contributor(initialName);

    await repository.AddAsync(Contributor);

    // detach the item so we get a different instance
    _dbContext.Entry(Contributor).State = EntityState.Detached;

    // fetch the item and update its title
    var newContributor = (await repository.ListAsync())
        .FirstOrDefault(Contributor => Contributor.Name == initialName);
    newContributor.ShouldNotBeNull();

    Contributor.ShouldNotBeSameAs(newContributor);
    var newName = Guid.NewGuid().ToString();
    newContributor.UpdateName(newName);

    // Update the item
    await repository.UpdateAsync(newContributor);

    // Fetch the updated item
    var updatedItem = (await repository.ListAsync())
        .FirstOrDefault(Contributor => Contributor.Name == newName);

    updatedItem.ShouldNotBeNull();
    Contributor.Name.ShouldNotBe(updatedItem.Name);
    Contributor.Status.ShouldBe(updatedItem.Status);
    newContributor.Id.ShouldBe(updatedItem.Id);
  }
}
