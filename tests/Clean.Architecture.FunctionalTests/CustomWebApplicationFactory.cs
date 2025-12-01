using Clean.Architecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace Clean.Architecture.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
  private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
    .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
    .WithPassword("Your_password123!")
    .Build();

  public async Task InitializeAsync()
  {
    await _dbContainer.StartAsync();
  }

  public new async Task DisposeAsync()
  {
    // Clean up environment variable
    Environment.SetEnvironmentVariable("USE_SQL_SERVER", null);
    await _dbContainer.DisposeAsync();
  }

  /// <summary>
  /// Overriding CreateHost to avoid creating a separate ServiceProvider per this thread:
  /// https://github.com/dotnet-architecture/eShopOnWeb/issues/465
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.UseEnvironment("Testing"); // will not send real emails
    var host = builder.Build();
    host.Start();

    // Get service provider.
    var serviceProvider = host.Services;

    // Create a scope to obtain a reference to the database
    // context (AppDbContext).
    using (var scope = serviceProvider.CreateScope())
    {
      var scopedServices = scope.ServiceProvider;
      var db = scopedServices.GetRequiredService<AppDbContext>();

      var logger = scopedServices
          .GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

      try
      {
        // Apply migrations to create the database schema
        db.Database.Migrate();
        
        // Seed the database with test data.
        SeedData.PopulateTestDataAsync(db).Wait();
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {exceptionMessage}", ex.Message);
      }
    }

    return host;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    // Force SQL Server mode even on non-Windows platforms for functional tests
    Environment.SetEnvironmentVariable("USE_SQL_SERVER", "true");
    
    builder
        .ConfigureAppConfiguration((context, config) =>
        {
          // Set the connection string to use the Testcontainer
          config.AddInMemoryCollection(new Dictionary<string, string?>
          {
            ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString()
          });
        })
        .ConfigureServices(services =>
        {
          // Remove the app's ApplicationDbContext registration
          var descriptors = services.Where(
            d => d.ServiceType == typeof(AppDbContext) ||
                 d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                .ToList();

          foreach (var descriptor in descriptors)
          {
            services.Remove(descriptor);
          }

          // Add ApplicationDbContext using the Testcontainers SQL Server instance
          services.AddDbContext<AppDbContext>((provider, options) =>
          {
            options.UseSqlServer(_dbContainer.GetConnectionString());
            var interceptor = provider.GetRequiredService<EventDispatchInterceptor>();
            options.AddInterceptors(interceptor);
          });
        });
  }
}
