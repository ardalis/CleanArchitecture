using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.Infrastructure.Data;

public static class SeedData
{
#pragma warning disable CA1707 // Preserve the established public constant name.
  public const int NUMBER_OF_CONTRIBUTORS = 27; // including the 2 below
#pragma warning restore CA1707

  public static readonly ContributorName Contributor1Name =
    ContributorName.From("Ardalis");

  public static readonly ContributorName Contributor2Name =
    ContributorName.From("Ilyana");

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    Guard.Against.Null(dbContext);

    var hasContributors = await dbContext.Contributors
      .AnyAsync()
      .ConfigureAwait(false);

    if (hasContributors)
    {
      return;
    }

    await PopulateTestDataAsync(dbContext)
      .ConfigureAwait(false);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    Guard.Against.Null(dbContext);

    await dbContext.Database
      .ExecuteSqlInterpolatedAsync(
        $"INSERT INTO [Contributors] ([Name], [Status]) VALUES ({Contributor1Name.Value}, {ContributorStatus.NotSet.Value})")
      .ConfigureAwait(false);

    await dbContext.Database
      .ExecuteSqlInterpolatedAsync(
        $"INSERT INTO [Contributors] ([Name], [Status]) VALUES ({Contributor2Name.Value}, {ContributorStatus.NotSet.Value})")
      .ConfigureAwait(false);

    for (var index = 1; index <= NUMBER_OF_CONTRIBUTORS - 2; index++)
    {
      var contributorName = $"Contributor {index}";

      await dbContext.Database
        .ExecuteSqlInterpolatedAsync(
          $"INSERT INTO [Contributors] ([Name], [Status]) VALUES ({contributorName}, {ContributorStatus.NotSet.Value})")
        .ConfigureAwait(false);
    }
  }
}
