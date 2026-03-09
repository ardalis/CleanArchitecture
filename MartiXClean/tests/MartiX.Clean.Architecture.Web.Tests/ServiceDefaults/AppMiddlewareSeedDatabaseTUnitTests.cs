using FastEndpoints;
using FastEndpoints.Swagger;
using MartiX.Clean.Architecture.Web.Configurations;
using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;
using MartiX.Clean.Architecture.Web.Infrastructure;
using MartiX.WebApi.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MartiX.Clean.Architecture.Web.Tests.ServiceDefaults;

public class AppMiddlewareSeedDatabaseTUnitTests
{
  [Test]
  public async Task UseAppMiddlewareAndSeedDatabase_WhenProductionInMemory_Completes()
  {
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = Environments.Production });
    builder.Services.AddLogging();
    builder.Services.AddScoped(_ => Substitute.For<IRepository<Product>>());
    builder.Services.AddMediatorSourceGen(Substitute.For<ILogger>());
    builder.Services.AddFastEndpoints().SwaggerDocument();
    TestDbContextHelper.AddInMemoryAppDbContext(builder.Services);
    var app = builder.Build();

    var configuredApp = await app.UseAppMiddlewareAndSeedDatabase();

    await Assert.That(ReferenceEquals(app, configuredApp)).IsTrue();
  }
}

