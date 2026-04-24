using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
  [Fact]
  public async Task AddsContributorAndSetsId()
  {
    var testContributorName = ContributorName.From("testContributor");
    var testContributorStatus = ContributorStatus.NotSet;
    var repository = GetRepository();
    var Contributor = new Contributor(testContributorName);

    await repository.AddAsync(Contributor);

    var newContributor = (await repository.ListAsync())
                    .FirstOrDefault();

    newContributor.ShouldNotBeNull();
    testContributorName.ShouldBe(newContributor.Name);
    testContributorStatus.ShouldBe(newContributor.Status);
    newContributor.Id.Value.ShouldBeGreaterThan(0);
  }

  [Fact]
  public async Task AddsTwoContributorsWithDistinctDbGeneratedIds()
  {
    var repository = GetRepository();
    var first = new Contributor(ContributorName.From("first"));
    var second = new Contributor(ContributorName.From("second"));

    await repository.AddAsync(first);
    await repository.AddAsync(second);

    var all = await repository.ListAsync();
    all.Count.ShouldBe(2);
    all[0].Id.Value.ShouldBeGreaterThan(0);
    all[1].Id.Value.ShouldBeGreaterThan(0);
    all[0].Id.ShouldNotBe(all[1].Id);
  }
}
