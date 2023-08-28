using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Web;

public static class SeedData
{
  public static readonly Contributor Contributor1 = new ("Ardalis");
  public static readonly Contributor Contributor2 = new ("Snowfrog");

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using (var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
    {
      // Look for any Contributors.
      if (dbContext.Contributors.Any())
      {
        return;   // DB has been seeded
      }

      PopulateTestData(dbContext);
    }
  }
  public static void PopulateTestData(AppDbContext dbContext)
  {
    foreach (var item in dbContext.Contributors)
    {
      dbContext.Remove(item);
    }
    dbContext.SaveChanges();

    dbContext.Contributors.Add(Contributor1);
    dbContext.Contributors.Add(Contributor2);

    dbContext.SaveChanges();
  }
}
