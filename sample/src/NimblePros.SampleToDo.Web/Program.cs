using System.Text;
using AspNetCore.Localizer.Json.Extensions;
using FluentValidation;
using Microsoft.Extensions.Localization;
using NimblePros.Metronome;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.Infrastructure.Data;
using NimblePros.SampleToDo.Web.Configurations;
using NimblePros.SampleToDo.Web.Projects;

public partial class Program
{
  private static async Task Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.ConfigureKestrel(o =>
    {
      o.AddServerHeader = false; // <- removes "Server: Kestrel"
    });
    
    var logger = Log.Logger = new LoggerConfiguration()
      .Enrich.FromLogContext()
      .WriteTo.Console()
      .CreateLogger();

    logger.Information("Starting web host");

    builder.AddLoggerConfigs();

    var appLogger = new SerilogLoggerFactory(logger)
        .CreateLogger<Program>();

    builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);

    builder.Services.AddServiceConfigs(appLogger, builder);

    builder.Services.AddFastEndpoints()
                    .SwaggerDocument(o =>
                    {
                      o.ShortSchemaNames = true;
                    });
    builder.Services.AddValidatorsFromAssemblyContaining<UpdateProjectRequestValidator>();

    if (!builder.Environment.EnvironmentName.Equals("Testing"))
    {
      var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
      builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));
    }

    builder.Services.AddMemoryCache();

    // add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
    builder.Services.Configure((Ardalis.ListStartupServices.ServiceConfig config) =>
    {
      config.Services = new List<ServiceDescriptor>(builder.Services);

      // optional - default path to view services is /listallservices - recommended to choose your own path
      config.Path = "/listservices";
    });

    // track db and external service calls
    builder.Services.AddMetronome();

    // Add localization with the JSON path
    builder.Services.AddLocalization(options => options.ResourcesPath = "i18n");

    // ----- Add JSON-specific localization
    builder.Services.AddJsonLocalization(options =>
    {
      options.ResourcesPath = "i18n";  // Path to the JSON files (e.g., i18n/en/Project.json)
      options.CacheDuration = TimeSpan.FromHours(1);  // Optional: Cache for performance
      options.FileEncoding = Encoding.UTF8; //Optional: Specify file encoding
    });

    // ----- Register the typed localizer for the Project aggregate -----
    builder.Services.AddSingleton<IStringLocalizer>(sp =>
    {
      var factory = sp.GetRequiredService<IStringLocalizerFactory>();
      // Creates a localizer for 'ProjectAggregate' – loads from i18n/{culture}/Project.json
      return factory.Create(typeof(ProjectErrorMessages));
    });

    var app = builder.Build();

    // ----- Set the static holder for Core access -----
    var localizer = app.Services.GetRequiredService<IStringLocalizer>();
    Localization.Current = new Localization.LocalizationContext(localizer);

    // ----- Add request localization middleware (before app.Run()) -----
    var supportedCultures = new[] { "en", "fr", "fa" };  // Add your cultures
    var localizationOptions = new RequestLocalizationOptions()
        .SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
    app.UseRequestLocalization(localizationOptions);

    // Verify validators are registered properly in development
    if (app.Environment.IsDevelopment())
    {
      using var scope = app.Services.CreateScope();
      var validatorsCount = scope.ServiceProvider.GetServices<IValidator<UpdateProjectRequest>>().Count();
      appLogger.LogInformation("Validators found: {validatorsCount}", validatorsCount);
    }
    
    // see Configurations/MiddlewareConfig.cs  
    await app.UseAppMiddleware();

    app.Run();
  }
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}
