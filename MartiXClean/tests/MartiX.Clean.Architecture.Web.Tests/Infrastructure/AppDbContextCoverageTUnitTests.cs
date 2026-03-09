using MartiX.Clean.Architecture.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MartiX.Clean.Architecture.Web.Tests.Infrastructure;

public class AppDbContextCoverageTUnitTests
{
  [Test]
  public async Task AddApplicationDbContext_WhenCalled_RegistersSqlServerOptions()
  {
    const string connectionString = "Server=localhost,1433;Database=AppDb;User ID=sa;Password=Test123$;TrustServerCertificate=true";
    var services = new ServiceCollection();
    services.AddApplicationDbContext(connectionString);
    using var provider = services.BuildServiceProvider();

    var options = provider.GetRequiredService<DbContextOptions<AppDbContext>>();
    await Assert.That(options.Extensions.Any(e => e.GetType().Name.Contains("SqlServerOptionsExtension", StringComparison.Ordinal))).IsTrue();
  }

  [Test]
  public async Task InitializeAsync_WhenCalledTwice_SeedsOnlyOnce()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var logger = Substitute.For<ILogger>();

    await SeedData.InitializeAsync(db, logger);
    var firstCount = await db.Products.CountAsync();
    await SeedData.InitializeAsync(db, logger);
    var secondCount = await db.Products.CountAsync();

    await Assert.That(firstCount != SeedData.NUMBER_OF_PRODUCTS).IsFalse();
    await Assert.That(secondCount != firstCount).IsFalse();
  }

  [Test]
  public async Task CreateDbContext_WhenEnvironmentConnectionStringSet_UsesConfiguredDatabase()
  {
    const string key = "ConnectionStrings__AppDb";
    const string value = "Server=localhost,1433;Database=FactoryDb;User ID=sa;Password=Test123$;TrustServerCertificate=true";
    Environment.SetEnvironmentVariable(key, value);
    try
    {
      var factory = new AppDbContextFactory();
      using var context = factory.CreateDbContext([]);
      var actual = context.Database.GetConnectionString();

      await Assert.That(actual is null || !actual.Contains("FactoryDb", StringComparison.Ordinal)).IsFalse();
    }
    finally
    {
      Environment.SetEnvironmentVariable(key, null);
    }
  }
}

