using Clean.Architecture.Infrastructure.Data;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace Clean.Architecture.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> :
  WebApplicationFactory<TProgram>,
  IAsyncLifetime
  where TProgram : class
{
  private TestDatabaseProvider _requestedDatabaseProvider =
    TestDatabaseProvider.Auto;

  private readonly string _sqliteDatabasePath = Path.Combine(
    Path.GetTempPath(),
    $"clean-architecture-functional-tests-{Guid.NewGuid():N}.db");

  private MsSqlContainer? _dbContainer;

  public CustomWebApplicationFactory()
  {
    ActiveDatabaseProvider = TestDatabaseProvider.Auto;
  }

  public TestDatabaseProvider ActiveDatabaseProvider { get; private set; }

  internal static CustomWebApplicationFactory<TProgram> CreateWithProvider(
    TestDatabaseProvider databaseProvider)
  {
    return new CustomWebApplicationFactory<TProgram>
    {
      _requestedDatabaseProvider = databaseProvider,
      ActiveDatabaseProvider = databaseProvider == TestDatabaseProvider.Sqlite
        ? TestDatabaseProvider.Sqlite
        : TestDatabaseProvider.Auto
    };
  }

  public async ValueTask InitializeAsync()
  {
    if (_requestedDatabaseProvider == TestDatabaseProvider.Sqlite)
    {
      ActiveDatabaseProvider = TestDatabaseProvider.Sqlite;
      return;
    }

    MsSqlContainer? container = null;

    try
    {
      container = new MsSqlBuilder(
          "mcr.microsoft.com/mssql/server:2025-latest")
        .WithPassword("Your_password123!")
        .Build();

      await container.StartAsync();
      _dbContainer = container;
      ActiveDatabaseProvider = TestDatabaseProvider.SqlServer;
    }
    catch (DockerUnavailableException) when (
      _requestedDatabaseProvider == TestDatabaseProvider.Auto)
    {
      await DisposeContainerAfterFailedStartAsync(container);
      ActiveDatabaseProvider = TestDatabaseProvider.Sqlite;
    }
    catch
    {
      await DisposeContainerAfterFailedStartAsync(container);
      throw;
    }
  }

  public override async ValueTask DisposeAsync()
  {
    Environment.SetEnvironmentVariable("USE_SQL_SERVER", null);

    if (_dbContainer != null)
    {
      await _dbContainer.DisposeAsync();
    }

    await base.DisposeAsync();

    if (File.Exists(_sqliteDatabasePath))
    {
      File.Delete(_sqliteDatabasePath);
    }

    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Overriding CreateHost to avoid creating a separate ServiceProvider per this thread:
  /// https://github.com/dotnet-architecture/eShopOnWeb/issues/465
  /// </summary>
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.UseEnvironment("Testing");

    var host = builder.Build();
    host.Start();

    using var scope = host.Services.CreateScope();

    var scopedServices = scope.ServiceProvider;
    var db = scopedServices.GetRequiredService<AppDbContext>();
    var logger = scopedServices.GetRequiredService<
      ILogger<CustomWebApplicationFactory<TProgram>>>();

    try
    {
      logger.LogInformation(
        "Functional tests are using the {DatabaseProvider} database provider.",
        ActiveDatabaseProvider);

      db.Database.EnsureCreated();

      SeedData.InitializeAsync(db).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
      logger.LogError(
        ex,
        "An error occurred seeding the database with test messages. Error: {ExceptionMessage}",
        ex.Message);
      throw;
    }

    return host;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    var useSqlServer = _dbContainer != null;

    if (_requestedDatabaseProvider == TestDatabaseProvider.SqlServer &&
        !useSqlServer)
    {
      throw new InvalidOperationException(
        "SQL Server was explicitly selected, but the test container has not " +
        "been initialized. Use the factory as an xUnit fixture or call " +
        "InitializeAsync before creating the client.");
    }

    ActiveDatabaseProvider = useSqlServer
      ? TestDatabaseProvider.SqlServer
      : TestDatabaseProvider.Sqlite;

    var connectionString = useSqlServer
      ? _dbContainer!.GetConnectionString()
      : $"Data Source={_sqliteDatabasePath}";

    builder
      .ConfigureAppConfiguration((_, config) =>
      {
        config.AddInMemoryCollection(
          new Dictionary<string, string?>
          {
            ["ConnectionStrings:DefaultConnection"] =
              useSqlServer ? connectionString : null,
            ["ConnectionStrings:SqliteConnection"] =
              useSqlServer ? null : connectionString
          });
      })
      .ConfigureServices(services =>
      {
        var descriptors = services
          .Where(
            descriptor =>
              descriptor.ServiceType == typeof(AppDbContext) ||
              descriptor.ServiceType ==
                typeof(DbContextOptions<AppDbContext>))
          .ToList();

        foreach (var descriptor in descriptors)
        {
          services.Remove(descriptor);
        }

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
          if (useSqlServer)
          {
            options.UseSqlServer(connectionString);
          }
          else
          {
            options.UseSqlite(connectionString);
          }

          var interceptor =
            provider.GetRequiredService<EventDispatchInterceptor>();

          options.AddInterceptors(interceptor);
        });
      });
  }

  private static async ValueTask DisposeContainerAfterFailedStartAsync(
    MsSqlContainer? container)
  {
    if (container == null)
    {
      return;
    }

    try
    {
      await container.DisposeAsync();
    }
    catch
    {
      // Preserve the original container-start exception or continue with SQLite.
    }
  }
}
