using NimblePros.Metronome;
using NimblePros.SampleToDo.Core;
using NimblePros.SampleToDo.Infrastructure;

namespace NimblePros.SampleToDo.Web.Configurations;

public static class ServiceConfig
{
  public static IServiceCollection AddServiceConfigs(this IServiceCollection services,
                                                     Microsoft.Extensions.Logging.ILogger logger,
                                                     WebApplicationBuilder builder)
  {
    services.AddCoreServices(logger)
            .AddInfrastructureServices(builder.Configuration, logger, builder.Environment.EnvironmentName)
            .AddMediatorSourceGen(logger);

    // add a default http client
    services.AddHttpClient("Default")
      .AddMetronomeHandler();

    return services;
  }
}
