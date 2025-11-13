using Ardalis.GuardClauses;
using MinimalClean.Architecture.Web.Infrastructure.Data;
using MinimalClean.Architecture.Web.Infrastructure.Data.Queries;
using MinimalClean.Architecture.Web.ProductFeatures.List;
using Microsoft.EntityFrameworkCore;

namespace MinimalClean.Architecture.Web.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    // Always use SQL Server from Aspire
    string? connectionString = config.GetConnectionString("AppDb");
    Guard.Against.Null(connectionString, "AppDb connection string is required. Make sure the application is running with Aspire.");

    services.AddScoped<EventDispatchInterceptor>();
    services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();

    services.AddDbContext<AppDbContext>((provider, options) =>
    {
      var eventDispatchInterceptor = provider.GetRequiredService<EventDispatchInterceptor>();
      
      options.UseSqlServer(connectionString);
      options.AddInterceptors(eventDispatchInterceptor);
    });

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListProductsQueryService, ListProductsQueryService>();

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
