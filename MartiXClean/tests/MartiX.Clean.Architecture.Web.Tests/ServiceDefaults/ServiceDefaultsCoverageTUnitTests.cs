using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace MartiX.Clean.Architecture.Web.Tests.ServiceDefaults;

public class ServiceDefaultsCoverageTUnitTests
{
  [Test]
  public async Task AddServiceDefaults_WhenInDevelopment_RegistersHealthChecksAndMapsEndpoints()
  {
    var hostBuilder = Host.CreateApplicationBuilder();
    hostBuilder.AddServiceDefaults();
    using var hostProvider = hostBuilder.Services.BuildServiceProvider();
    var healthCheckService = hostProvider.GetService<HealthCheckService>();

    await Assert.That(healthCheckService == null).IsFalse();
    var appBuilder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = Environments.Development });
    appBuilder.AddDefaultHealthChecks();
    var app = appBuilder.Build();
    app.MapDefaultEndpoints();
  }
}

