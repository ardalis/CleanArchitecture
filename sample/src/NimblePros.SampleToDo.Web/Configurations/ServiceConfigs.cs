using NimblePros.SampleToDo.Infrastructure;
using NimblePros.SampleToDo.Core;

namespace NimblePros.SampleToDo.Web.Configurations;

public static class ServiceConfig
{
  public static IServiceCollection AddServiceConfigs(this IServiceCollection services,
                                                     Microsoft.Extensions.Logging.ILogger logger,
                                                     WebApplicationBuilder builder)
  {
    services.AddCoreServices(logger)
            .AddInfrastructureServices(builder.Configuration, logger, builder.Environment.EnvironmentName)
            .AddMediatrConfigs();

    logger.LogInformation("{Project} services registered", "Core and Infrastructure services registered");

    return services;
  }
}
