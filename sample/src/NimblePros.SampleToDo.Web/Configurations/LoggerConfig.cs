using Serilog;

namespace NimblePros.SampleToDo.Web.Configurations;

public static class LoggerConfig
{
  public static WebApplicationBuilder AddLoggerConfigs(this WebApplicationBuilder builder)
  {

    builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

    return builder;
  }
}
