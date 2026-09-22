using Clean.Architecture.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder
  .AddServiceDefaults()
  .AddLoggerConfigs();

using var loggerFactory = LoggerFactory.Create(
  config => config.AddConsole());

var startupLogger = loggerFactory.CreateLogger<Program>();

LogStartingWebHost(startupLogger);

builder.Services.AddOptionConfigs(
  builder.Configuration,
  startupLogger,
  builder);

builder.Services.AddServiceConfigs(
  startupLogger,
  builder);

builder.Services
  .AddFastEndpoints()
  .SwaggerDocument(options =>
  {
    options.DocumentSettings = settings =>
    {
      settings.Title = "Clean Architecture API";
      settings.Version = "v1";
      settings.Description =
        "HTTP endpoints for the Clean Architecture sample application.";
    };

    options.ShortSchemaNames = true;
  });

var app = builder.Build();

await app
  .UseAppMiddlewareAndSeedDatabase()
  .ConfigureAwait(false);

app.MapDefaultEndpoints();

await app
  .RunAsync()
  .ConfigureAwait(false);

// Make the implicit Program class public so integration tests can reference it.
public partial class Program
{
  [Microsoft.Extensions.Logging.LoggerMessage(
    EventId = 1,
    Level = Microsoft.Extensions.Logging.LogLevel.Information,
    Message = "Starting web host")]
  private static partial void LogStartingWebHost(
    Microsoft.Extensions.Logging.ILogger logger);
}
