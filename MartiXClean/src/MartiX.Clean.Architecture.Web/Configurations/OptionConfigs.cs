using MartiX.Clean.Architecture.Web.Infrastructure.Email;

namespace MartiX.Clean.Architecture.Web.Configurations;

public static class OptionConfigs
{
  public static IServiceCollection AddOptionConfigs(this IServiceCollection services,
                                                    IConfiguration configuration,
                                                    Microsoft.Extensions.Logging.ILogger logger,
                                                    WebApplicationBuilder builder)
  {
    services.Configure<MailserverConfiguration>(configuration.GetSection("Mailserver"))
    .Configure<DatabaseOptions>(configuration.GetSection("DatabaseOptions"))
    // Configure Web Behavior
    .Configure<CookiePolicyOptions>(options =>
    {
      options.CheckConsentNeeded = context => true;
      options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    logger.LogInformation("{Project} were configured", "Options");

    return services;
  }
}
