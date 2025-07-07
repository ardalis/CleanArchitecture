using NimblePros.SampleToDo.Infrastructure.Email;

namespace NimblePros.SampleToDo.Web.Configurations;

public static class OptionConfig
{
  public static IServiceCollection AddOptionConfigs(this IServiceCollection services,
                                                    IConfiguration configuration,
                                                    Microsoft.Extensions.Logging.ILogger logger,
                                                    WebApplicationBuilder builder)
  {
    services.Configure<MailserverConfiguration>(configuration.GetSection("Mailserver"));

    services.Configure<CookiePolicyOptions>(options =>
    {
      options.CheckConsentNeeded = context => true;
      options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    if (builder.Environment.IsDevelopment())
    {
      // add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
      services.Configure((Ardalis.ListStartupServices.ServiceConfig config) =>
      {
        config.Services = new List<ServiceDescriptor>(builder.Services);

        // optional - default path to view services is /listallservices - recommended to choose your own path
        config.Path = "/listservices";
      });
    }

    logger.LogInformation("{Project} were configured", "Configuration and Options");

    return services;
  }
}
