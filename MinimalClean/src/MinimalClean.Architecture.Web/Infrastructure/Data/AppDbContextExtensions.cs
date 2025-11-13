using Microsoft.EntityFrameworkCore;

namespace MinimalClean.Architecture.Web.Infrastructure.Data;

public static class AppDbContextExtensions
{
  public static void AddApplicationDbContext(this IServiceCollection services, string connectionString) =>
    services.AddDbContext<AppDbContext>(options =>
         options.UseSqlServer(connectionString));

}
