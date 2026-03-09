using MartiX.Clean.Architecture.Web.Configurations;
using MartiX.Clean.Architecture.Web.Domain.Interfaces;
using MartiX.Clean.Architecture.Web.Infrastructure.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MartiX.Clean.Architecture.Web.Tests.Configurations;

public class ServiceConfigsTUnitTests
{
  [Test]
  public async Task AddServiceConfigs_WhenInProduction_RegistersMimeKitEmailSender()
  {
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = Environments.Production });
    builder.Configuration["ConnectionStrings:AppDb"] = "Server=localhost,1433;Database=AppDb;User ID=sa;Password=Test123$;TrustServerCertificate=true";
    var logger = Substitute.For<ILogger>();

    builder.Services.AddServiceConfigs(logger, builder);
    using var provider = builder.Services.BuildServiceProvider();
    var sender = provider.GetRequiredService<IEmailSender>();

    await Assert.That(sender is not MimeKitEmailSender).IsFalse();
  }
}

