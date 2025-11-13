using Serilog;

namespace Clean.Architecture.Web.Configurations;

public static class LoggerConfigs
{
  public static WebApplicationBuilder AddLoggerConfigs(this WebApplicationBuilder builder)
  {
    // Add Serilog as an additional logging provider alongside OpenTelemetry
    // This allows both Serilog (for console/file) and OpenTelemetry (for Aspire) to work together
    builder.Logging.AddSerilog(new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .Enrich.FromLogContext()
      .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
      .WriteTo.Console()
      .CreateLogger());

    return builder;
  }
}
