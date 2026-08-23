using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure;
using Clean.Architecture.Infrastructure.Email;

namespace Clean.Architecture.Web.Configurations;

public static partial class ServiceConfigs
{
  public static IServiceCollection AddServiceConfigs(
    this IServiceCollection services,
    Microsoft.Extensions.Logging.ILogger logger,
    WebApplicationBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentNullException.ThrowIfNull(logger);
    ArgumentNullException.ThrowIfNull(builder);

    services
      .AddInfrastructureServices(builder.Configuration, logger)
      .AddMediatorSourceGen(logger);

    if (builder.Environment.IsDevelopment())
    {
      // Use a local test email server configured in Aspire.
      services.AddScoped<IEmailSender, MimeKitEmailSender>();
    }
    else
    {
      services.AddScoped<IEmailSender, MimeKitEmailSender>();
    }

    LogServicesRegistered(logger);

    return services;
  }

  [LoggerMessage(
    EventId = 1,
    Level = LogLevel.Information,
    Message = "Mediator source generator and email sender services registered")]
  private static partial void LogServicesRegistered(
    Microsoft.Extensions.Logging.ILogger logger);
}
