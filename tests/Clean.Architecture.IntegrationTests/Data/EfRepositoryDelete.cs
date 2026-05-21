using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.IntegrationTests.Data;

public class EfRepositoryDelete : BaseEfRepoTestFixture
{
  [Fact]
  public async Task DeletesItemAfterAddingIt()
  {
    var cancellationToken = TestContext.Current.CancellationToken;
    // add a Contributor
    var repository = GetRepository();
    var initialName = ContributorName.From(Guid.NewGuid().ToString());
    var Contributor = new Contributor(initialName);
    await repository.AddAsync(Contributor, cancellationToken);

    // delete the item
    await repository.DeleteAsync(Contributor, cancellationToken);

    // verify it's no longer there
    (await repository.ListAsync(cancellationToken)).ShouldNotContain(Contributor => Contributor.Name == initialName);
  }
}
