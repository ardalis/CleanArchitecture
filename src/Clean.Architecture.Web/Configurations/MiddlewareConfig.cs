using Ardalis.ListStartupServices;
using Clean.Architecture.Infrastructure.Data;
using Scalar.AspNetCore;

namespace Clean.Architecture.Web.Configurations;

public static partial class MiddlewareConfig
{
  public static async Task<IApplicationBuilder> UseAppMiddlewareAndSeedDatabase(
    this WebApplication app)
  {
    ArgumentNullException.ThrowIfNull(app);

    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseShowAllServicesMiddleware();
    }
    else
    {
      app.UseDefaultExceptionHandler();
      app.UseHsts();
    }

    app.UseFastEndpoints();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwaggerGen(
        options =>
        {
          options.Path = "/openapi/{documentName}.json";
        },
        settings =>
        {
          settings.Path = "/swagger";
          settings.DocumentPath = "/openapi/{documentName}.json";
        });

      app.MapScalarApiReference(options =>
      {
        options.WithTitle("Clean Architecture API");
        options.WithOpenApiRoutePattern("/openapi/{documentName}.json");
      });
    }

    app.UseHttpsRedirection();

    var shouldMigrate =
      app.Environment.IsDevelopment() ||
      app.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup");

    if (shouldMigrate)
    {
      await MigrateDatabaseAsync(app)
        .ConfigureAwait(false);

      await SeedDatabaseAsync(app)
        .ConfigureAwait(false);
    }

    return app;
  }

  private static async Task MigrateDatabaseAsync(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
      LogApplyingDatabaseMigrations(logger);

      var context = services.GetRequiredService<AppDbContext>();

      if (context.Database.IsSqlite())
      {
        await context.Database
          .EnsureCreatedAsync()
          .ConfigureAwait(false);

        LogSqliteDatabaseCreated(logger);
      }
      else
      {
        await context.Database
          .MigrateAsync()
          .ConfigureAwait(false);

        LogDatabaseMigrationsApplied(logger);
      }
    }
    catch (Exception exception)
    {
      LogDatabaseMigrationError(logger, exception);
      throw;
    }
  }

  private static async Task SeedDatabaseAsync(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
      LogSeedingDatabase(logger);

      var context = services.GetRequiredService<AppDbContext>();

      await SeedData
        .InitializeAsync(context)
        .ConfigureAwait(false);

      LogDatabaseSeeded(logger);
    }
#pragma warning disable CA1031 // Seeding failures are intentionally non-fatal.
    catch (Exception exception)
    {
      LogDatabaseSeedingError(logger, exception);
    }
#pragma warning restore CA1031
  }

  [LoggerMessage(
    EventId = 1,
    Level = LogLevel.Information,
    Message = "Applying database migrations...")]
  private static partial void LogApplyingDatabaseMigrations(
    Microsoft.Extensions.Logging.ILogger logger);

  [LoggerMessage(
    EventId = 2,
    Level = LogLevel.Information,
    Message = "SQLite database created successfully")]
  private static partial void LogSqliteDatabaseCreated(
    Microsoft.Extensions.Logging.ILogger logger);

  [LoggerMessage(
    EventId = 3,
    Level = LogLevel.Information,
    Message = "Database migrations applied successfully")]
  private static partial void LogDatabaseMigrationsApplied(
    Microsoft.Extensions.Logging.ILogger logger);

  [LoggerMessage(
    EventId = 4,
    Level = LogLevel.Error,
    Message = "An error occurred migrating the database.")]
  private static partial void LogDatabaseMigrationError(
    Microsoft.Extensions.Logging.ILogger logger,
    Exception exception);

  [LoggerMessage(
    EventId = 5,
    Level = LogLevel.Information,
    Message = "Seeding database...")]
  private static partial void LogSeedingDatabase(
    Microsoft.Extensions.Logging.ILogger logger);

  [LoggerMessage(
    EventId = 6,
    Level = LogLevel.Information,
    Message = "Database seeded successfully")]
  private static partial void LogDatabaseSeeded(
    Microsoft.Extensions.Logging.ILogger logger);

  [LoggerMessage(
    EventId = 7,
    Level = LogLevel.Error,
    Message = "An error occurred seeding the database.")]
  private static partial void LogDatabaseSeedingError(
    Microsoft.Extensions.Logging.ILogger logger,
    Exception exception);
}
