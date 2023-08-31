using Clean.Architecture.Core.ContributorAggregate;
using Microsoft.EntityFrameworkCore;
using Xunit;

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
    if (newContributor == null)
    {
      Assert.NotNull(newContributor);
      return;
    }
    Assert.NotSame(Contributor, newContributor);
    var newName = Guid.NewGuid().ToString();
    newContributor.UpdateName(newName);

    // Update the item
    await repository.UpdateAsync(newContributor);

    // Fetch the updated item
    var updatedItem = (await repository.ListAsync())
        .FirstOrDefault(Contributor => Contributor.Name == newName);

    Assert.NotNull(updatedItem);
    Assert.NotEqual(Contributor.Name, updatedItem?.Name);
    Assert.Equal(Contributor.Status, updatedItem?.Status);
    Assert.Equal(newContributor.Id, updatedItem?.Id);
  }
}
