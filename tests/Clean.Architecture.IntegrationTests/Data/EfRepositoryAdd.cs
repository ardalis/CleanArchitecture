using Clean.Architecture.Core.ContributorAggregate;
using Xunit;

namespace Clean.Architecture.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
  [Fact]
  public async Task AddsContributorAndSetsId()
  {
    const string testContributorName = "testContributor";
    ContributorStatus testContributorStatus = ContributorStatus.NotSet;
    Infrastructure.Data.EfRepository<Contributor> repository = GetRepository();
    var Contributor = new Contributor(testContributorName);

    await repository.AddAsync(Contributor);

    Contributor? newContributor = (await repository.ListAsync())
                    .FirstOrDefault();

    Assert.Equal(testContributorName, newContributor?.Name);
    Assert.Equal(testContributorStatus, newContributor?.Status);
    Assert.True(newContributor?.Id > 0);
  }
}
