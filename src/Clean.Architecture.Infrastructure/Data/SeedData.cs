using Clean.Architecture.Core.ContributorAggregate;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Infrastructure.Data;

public static class SeedData
{
  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Contributors.AnyAsync()) return; // DB has been seeded

    await PopulateTestDataAsync(dbContext);
  }

  private static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    Contributor[] Contributors = [new("Ardalis"), new("Snowfrog")];

    dbContext.Contributors.AddRange(Contributors);
    await dbContext.SaveChangesAsync();
  }
}
