using System.Globalization;
using Serilog;

namespace Clean.Architecture.Web.Configurations;

public static class LoggerConfigs
{
  public static WebApplicationBuilder AddLoggerConfigs(
    this WebApplicationBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(builder);

    // Add Serilog as an additional logging provider alongside OpenTelemetry.
    // This allows Serilog and OpenTelemetry to work together.
    builder.Logging.AddSerilog(
      new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty(
          "Application",
          builder.Environment.ApplicationName)
        .WriteTo.Console(
          formatProvider: CultureInfo.InvariantCulture)
        .CreateLogger());

    return builder;
  }
}
