using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.Infrastructure.Data;

public static class SeedData
{
  public const int NUMBER_OF_CONTRIBUTORS = 27; // including the 2 below
  public static readonly Contributor Contributor1 = new(ContributorName.From("Ardalis"));
  public static readonly Contributor Contributor2 = new(ContributorName.From("Ilyana"));

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Contributors.AnyAsync()) return; // DB has been seeded

    await PopulateTestDataAsync(dbContext);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    dbContext.Contributors.AddRange([Contributor1, Contributor2]);
    await dbContext.SaveChangesAsync();

    // add a bunch more contributors to support demonstrating paging
    for (int i = 1; i <= NUMBER_OF_CONTRIBUTORS-2; i++)
    {
      dbContext.Contributors.Add(new Contributor(ContributorName.From($"Contributor {i}")));
    }
    await dbContext.SaveChangesAsync();
  }
}
