using MartiX.Clean.Architecture.Web.Configurations;
using MartiX.Clean.Architecture.Web.Domain.Interfaces;
using MartiX.Clean.Architecture.Web.Infrastructure.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace MartiX.Clean.Architecture.Web.Tests.Configurations;

public class ConfigurationBindingsTUnitTests
{
  [Test]
  public void AddOptionConfigs_WhenConfigurationProvided_BindsMailAndDatabaseOptions()
  {
    var builder = WebApplication.CreateBuilder();
    var config = new ConfigurationBuilder()
      .AddInMemoryCollection(new Dictionary<string, string?>
      {
        ["Mailserver:Hostname"] = "smtp.local",
        ["Mailserver:Port"] = "2525",
        ["DatabaseOptions:RecreateOnStartup"] = "true"
      })
      .Build();
    var logger = Substitute.For<ILogger>();
    var services = new ServiceCollection();

    services.AddOptionConfigs(config, logger, builder);
    using var provider = services.BuildServiceProvider();
    var mailOptions = provider.GetRequiredService<IOptions<MailserverConfiguration>>().Value;
    var dbOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

    if (mailOptions.Hostname != "smtp.local" || mailOptions.Port != 2525) throw new Exception("Mail options were not bound.");
    if (!dbOptions.RecreateOnStartup) throw new Exception("Database options were not bound.");
  }

  [Test]
  public void AddServiceConfigs_WhenInDevelopment_RegistersMimeKitEmailSender()
  {
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = Environments.Development });
    builder.Configuration["ConnectionStrings:AppDb"] = "Server=localhost,1433;Database=AppDb;User ID=sa;Password=Test123$;TrustServerCertificate=true";
    var logger = Substitute.For<ILogger>();

    builder.Services.AddServiceConfigs(logger, builder);
    using var provider = builder.Services.BuildServiceProvider();
    var sender = provider.GetRequiredService<IEmailSender>();

    if (sender is not MimeKitEmailSender) throw new Exception("Expected MimeKitEmailSender registration.");
  }

  [Test]
  public async Task AddLoggerConfigs_WhenCalled_Completes()
  {
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = Environments.Development });
    builder.AddLoggerConfigs();
    await Task.CompletedTask;
  }
}

