using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure;
using Clean.Architecture.Infrastructure.Email;

namespace Clean.Architecture.Web.Configurations;

public static class ServiceConfigs
{
  public static IServiceCollection AddServiceConfigs(this IServiceCollection services, Microsoft.Extensions.Logging.ILogger logger, WebApplicationBuilder builder)
  {
    services.AddInfrastructureServices(builder.Configuration, logger)
            .AddMediatorSourceGen(logger);

    if (builder.Environment.IsDevelopment())
    {
      // Use a local test email server - configured in Aspire
      // See: https://ardalis.com/configuring-a-local-test-email-server/
      services.AddScoped<IEmailSender, MimeKitEmailSender>();

      // Otherwise use this:
      //builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
    }
    else
    {
      services.AddScoped<IEmailSender, MimeKitEmailSender>();
    }

    logger.LogInformation("{Project} services registered", "Mediator Source Generator and Email Sender");

    return services;
  }


}
