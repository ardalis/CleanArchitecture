using Ardalis.ListStartupServices;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalClean.Architecture.Web.Infrastructure.Data;
using Scalar.AspNetCore;

namespace MinimalClean.Architecture.Web.Configurations;

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
      });
      app.MapScalarApiReference();
    }

    app.UseHttpsRedirection(); // Note this will drop Authorization headers

    await SeedDatabase(app);

    return app;
  }

  static async Task SeedDatabase(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
      var context = services.GetRequiredService<AppDbContext>();
      var dbOptions = services.GetService<IOptions<DatabaseOptions>>()?.Value;
      
      // Drop and recreate database in development if configured
      if (app.Environment.IsDevelopment() && dbOptions?.RecreateOnStartup == true)
      {
        logger.LogWarning("DROPPING database for fresh start (DatabaseOptions:RecreateOnStartup = true)...");
        await context.Database.EnsureDeletedAsync();
        logger.LogInformation("Database dropped.");
      }

      // Apply all pending migrations
      logger.LogInformation("Applying database migrations...");
      await context.Database.MigrateAsync();
      logger.LogInformation("Database migrations applied successfully.");

      // Seed data
      await SeedData.InitializeAsync(context, logger);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
    }
  }
}
