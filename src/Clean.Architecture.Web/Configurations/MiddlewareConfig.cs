using Ardalis.ListStartupServices;
using Clean.Architecture.Infrastructure.Data;
using Scalar.AspNetCore;

namespace Clean.Architecture.Web.Configurations;

public static class MiddlewareConfig
{
  public static async Task<IApplicationBuilder> UseAppMiddlewareAndSeedDatabase(this WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
    }
    else
    {   
      app.UseDefaultExceptionHandler(); // from FastEndpoints
      app.UseHsts();
    }

    app.UseFastEndpoints();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwaggerGen(options =>
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

    app.UseHttpsRedirection(); // Note this will drop Authorization headers

    // Run migrations and seed in Development or when explicitly requested via environment variable
    var shouldMigrate = app.Environment.IsDevelopment() || 
                        app.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup");
    
    if (shouldMigrate)
    {
      await MigrateDatabaseAsync(app);
      await SeedDatabaseAsync(app);
    }

    return app;
  }

  static async Task MigrateDatabaseAsync(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
      logger.LogInformation("Applying database migrations...");
      var context = services.GetRequiredService<AppDbContext>();
      
      // For SQLite, use EnsureCreated instead of migrations (common for dev/local scenarios)
      // For SQL Server, use migrations (production scenario)
      if (context.Database.IsSqlite())
      {
        await context.Database.EnsureCreatedAsync();
        logger.LogInformation("SQLite database created successfully");
      }
      else
      {
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully");
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred migrating the DB. {exceptionMessage}", ex.Message);
      throw; // Re-throw to make startup fail if migrations fail
    }
  }

  static async Task SeedDatabaseAsync(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
      logger.LogInformation("Seeding database...");
      var context = services.GetRequiredService<AppDbContext>();
      await SeedData.InitializeAsync(context);
      logger.LogInformation("Database seeded successfully");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
      // Don't re-throw for seeding errors - it's not critical
    }
  }
}
