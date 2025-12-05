using Clean.Architecture.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()    // This sets up OpenTelemetry logging
       .AddLoggerConfigs();     // This adds Serilog for console formatting

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

startupLogger.LogInformation("Starting web host");

builder.Services.AddOptionConfigs(builder.Configuration, startupLogger, builder);
builder.Services.AddServiceConfigs(startupLogger, builder);

builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.DocumentSettings = s =>
                  {
                    s.Title = "Clean Architecture API";
                    s.Version = "v1";
                    s.Description = "HTTP endpoints for the Clean Architecture sample application.";
                  };
                  o.ShortSchemaNames = true;
                });

var app = builder.Build();

await app.UseAppMiddlewareAndSeedDatabase();

app.MapDefaultEndpoints(); // Aspire health checks and metrics

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
