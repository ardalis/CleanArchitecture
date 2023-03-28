using Clean.Architecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Architecture.Infrastructure;

public static class StartupSetup
{
  /*
  public static void AddDbContext(this IServiceCollection services, string connectionString) =>
      services.AddDbContext<AppDbContext>(options =>
          options.UseSqlite(connectionString)); // will be created in web project root
  */
  
  public static void AddDbContext(this IServiceCollection services, string? connectionString = null)
  {
    if (connectionString == null)
    {
      var builder = new SqliteConnectionStringBuilder()
      {
        DataSource = "Your database file",
        DefaultTimeout = 10,
      };

      connectionString = builder.ConnectionString;
    }

    services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
  }
}
