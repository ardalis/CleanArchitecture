using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
  [Fact]
  public async Task AddsContributorAndSetsId()
  {
    var cancellationToken = TestContext.Current.CancellationToken;
    var testContributorName = ContributorName.From("testContributor");
    var testContributorStatus = ContributorStatus.NotSet;
    var repository = GetRepository();
    var Contributor = new Contributor(testContributorName);

    await repository.AddAsync(Contributor, cancellationToken);

    var newContributor = (await repository.ListAsync(cancellationToken))
                    .FirstOrDefault();

    newContributor.ShouldNotBeNull();
    testContributorName.ShouldBe(newContributor.Name);
    testContributorStatus.ShouldBe(newContributor.Status);
    newContributor.Id.Value.ShouldBeGreaterThan(0);
  }

  [Fact]
  public async Task AddsTwoContributorsWithDistinctDbGeneratedIds()
  {
    var cancellationToken = TestContext.Current.CancellationToken;
    var repository = GetRepository();
    var first = new Contributor(ContributorName.From("first"));
    var second = new Contributor(ContributorName.From("second"));

    await repository.AddAsync(first, cancellationToken);
    await repository.AddAsync(second, cancellationToken);

    var all = await repository.ListAsync(cancellationToken);
    all.Count.ShouldBe(2);
    all[0].Id.Value.ShouldBeGreaterThan(0);
    all[1].Id.Value.ShouldBeGreaterThan(0);
    all[0].Id.ShouldNotBe(all[1].Id);
  }
}
