using NimblePros.Metronome;
using NimblePros.SampleToDo.Infrastructure.Data;

namespace NimblePros.SampleToDo.Web.Configurations;

public static class MiddlewareConfig
{
  public static async Task<IApplicationBuilder> UseAppMiddleware(this WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
      app.UseMetronomeLoggingMiddleware();
    }
    else
    {
      app.UseDefaultExceptionHandler(); // from FastEndpoints
      app.UseHsts();
    }

    app.UseFastEndpoints()
        .UseSwaggerGen(); // Includes AddFileServer and static files middleware

    app.UseHttpsRedirection();

    await SeedDatabase(app);

    return app;
  }

  static async Task SeedDatabase(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
      var context = services.GetRequiredService<AppDbContext>();
      //          context.Database.Migrate();
      context.Database.EnsureCreated();
      await SeedData.InitializeAsync(context);
    }
    catch (Exception ex)
    {
      var logger = services.GetRequiredService<ILogger<Program>>();
      logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
    }
  }
}
