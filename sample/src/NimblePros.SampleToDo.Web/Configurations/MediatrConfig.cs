using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Infrastructure;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;

namespace NimblePros.SampleToDo.Web.Configurations;

public static class MediatrConfig
{
  public static IServiceCollection AddMediator(this IServiceCollection services)
  {
    services.AddMediator(options =>
    {
      options.Assemblies = [Assembly.GetExecutingAssembly()];
    });
    return services;
  }

  public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
  {
    var mediatRAssemblies = new[]
      {
        Assembly.GetAssembly(typeof(Contributor)), // Core
        Assembly.GetAssembly(typeof(CreateContributorCommand)), // UseCases
        Assembly.GetAssembly(typeof(InfrastructureServiceExtensions)) // Infrastructure
      };

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

    return services;
  }
}
