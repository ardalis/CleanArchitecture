using Clean.Architecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace Clean.Architecture.FunctionalTests;

public class CustomWebApplicationFactory<TProgram>
  : WebApplicationFactory<TProgram>, IAsyncLifetime
  where TProgram : class
{
  private static readonly Action<ILogger, string, Exception?> _logDatabaseSeedingError =
    LoggerMessage.Define<string>(
      LogLevel.Error,
      new EventId(1, "DatabaseSeeding"),
      "An error occurred seeding the database with test messages. Error: {ExceptionMessage}");

  private MsSqlContainer? _dbContainer;

  public async ValueTask InitializeAsync()
  {
    try
    {
      _dbContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-latest")
        .WithPassword("Your_password123!")
        .Build();

      await _dbContainer.StartAsync().ConfigureAwait(false);
    }
    catch (ArgumentException)
    {
      // Docker is unavailable; use the SQLite configuration.
      _dbContainer = null;
    }
  }

  public override async ValueTask DisposeAsync()
  {
    Environment.SetEnvironmentVariable("USE_SQL_SERVER", null);

    if (_dbContainer is not null)
    {
      await _dbContainer.DisposeAsync().ConfigureAwait(false);
    }

    await base.DisposeAsync().ConfigureAwait(false);
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Overrides CreateHost to avoid creating a separate service provider.
  /// </summary>
  protected override IHost CreateHost(IHostBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(builder);

    builder.UseEnvironment("Testing");

    var host = builder.Build();
    host.Start();

    var serviceProvider = host.Services;

    using var scope = serviceProvider.CreateScope();
    var scopedServices = scope.ServiceProvider;
    var db = scopedServices.GetRequiredService<AppDbContext>();

    var logger = scopedServices
      .GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

    try
    {
      // Functional tests use EnsureCreated to avoid migration-script coupling.
      db.Database.EnsureCreated();

      // CreateHost is synchronous, so wait for seeding before disposing the scope.
#pragma warning disable CA2025
      SeedData.InitializeAsync(db).GetAwaiter().GetResult();
#pragma warning restore CA2025
    }
    catch (Exception exception)
    {
      _logDatabaseSeedingError(logger, exception.Message, exception);
      throw;
    }

    return host;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(builder);

    if (_dbContainer is not null)
    {
      Environment.SetEnvironmentVariable("USE_SQL_SERVER", "true");
    }

    builder
      .ConfigureAppConfiguration((context, config) =>
      {
        if (_dbContainer is not null)
        {
          config.AddInMemoryCollection(new Dictionary<string, string?>
          {
            ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString()
          });
        }
      })
      .ConfigureServices(services =>
      {
        if (_dbContainer is null)
        {
          return;
        }

        var descriptors = services.Where(
          descriptor =>
            descriptor.ServiceType == typeof(AppDbContext) ||
            descriptor.ServiceType == typeof(DbContextOptions<AppDbContext>))
          .ToList();

        foreach (var descriptor in descriptors)
        {
          services.Remove(descriptor);
        }

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
          options.UseSqlServer(_dbContainer.GetConnectionString());

          var interceptor =
            provider.GetRequiredService<EventDispatchInterceptor>();

          options.AddInterceptors(interceptor);
        });
      });
  }
}
