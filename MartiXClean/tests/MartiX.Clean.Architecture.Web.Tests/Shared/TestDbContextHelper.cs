using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MartiX.Clean.Architecture.Web.Infrastructure.Data;

namespace MartiX.Clean.Architecture.Web.Tests;

public static class TestDbContextHelper
{
  public static AppDbContext CreateInMemoryAppDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
      .Options;

    return new AppDbContext(options);
  }

  public static void AddInMemoryAppDbContext(IServiceCollection services) =>
    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString("N")));
}
