using Microsoft.EntityFrameworkCore;
using MinimalClean.Architecture.Web.Domain.ProductAggregate;
using Microsoft.Extensions.Logging;

namespace MinimalClean.Architecture.Web.Infrastructure.Data;

public static class SeedData
{
  public const int NUMBER_OF_PRODUCTS = 10;

  public static async Task InitializeAsync(AppDbContext dbContext, ILogger logger)
  {
    if (await dbContext.Products.AnyAsync())
    {
      logger.LogInformation("DB has data - seeding not required.");
      return; // DB has been seeded
    }
    await PopulateTestDataAsync(dbContext, logger);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext, ILogger logger)
  {
    logger.LogInformation("Seeding database with sample data.");

    // add more products to support demonstrating paging
    for (int i = 1; i <= NUMBER_OF_PRODUCTS; i++)
    {
      dbContext.Products.Add(new Product(ProductId.From(i), $"Product {i}", 10m + i));
    }
    await dbContext.SaveChangesAsync();
  }
}
