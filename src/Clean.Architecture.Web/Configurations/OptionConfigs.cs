using Ardalis.ListStartupServices;
using Clean.Architecture.Infrastructure.Email;

namespace Clean.Architecture.Web.Configurations;

public static partial class OptionConfigs
{
  public static IServiceCollection AddOptionConfigs(
    this IServiceCollection services,
    IConfiguration configuration,
    Microsoft.Extensions.Logging.ILogger logger,
    WebApplicationBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentNullException.ThrowIfNull(configuration);
    ArgumentNullException.ThrowIfNull(logger);
    ArgumentNullException.ThrowIfNull(builder);

    services
      .Configure<MailserverConfiguration>(
        configuration.GetSection("Mailserver"))
      .Configure<CookiePolicyOptions>(options =>
      {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

    if (builder.Environment.IsDevelopment())
    {
      // Add list services for diagnostic purposes.
      services.Configure<ServiceConfig>(config =>
      {
        config.Services = new List<ServiceDescriptor>(builder.Services);
        config.Path = "/listservices";
      });
    }

    LogOptionsConfigured(logger);

    return services;
  }

  [LoggerMessage(
    EventId = 1,
    Level = LogLevel.Information,
    Message = "Options were configured")]
  private static partial void LogOptionsConfigured(
    Microsoft.Extensions.Logging.ILogger logger);
}
