using MartiX.Clean.Architecture.Web.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace MartiX.Clean.Architecture.Web.Tests.Configurations;

public class OptionConfigsTUnitTests
{
  [Test]
  public async Task AddOptionConfigs_WhenCalled_ConfiguresCookiePolicyDefaults()
  {
    var builder = WebApplication.CreateBuilder();
    var config = new ConfigurationBuilder().Build();
    var logger = Substitute.For<ILogger>();
    var services = new ServiceCollection();

    services.AddOptionConfigs(config, logger, builder);

    using var provider = services.BuildServiceProvider();
    var cookieOptions = provider.GetRequiredService<IOptions<CookiePolicyOptions>>().Value;

    await Assert.That(cookieOptions.CheckConsentNeeded is null || !cookieOptions.CheckConsentNeeded(new DefaultHttpContext())).IsFalse();
    await Assert.That(cookieOptions.MinimumSameSitePolicy != SameSiteMode.None).IsFalse();
  }
}

