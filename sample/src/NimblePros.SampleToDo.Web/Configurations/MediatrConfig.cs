using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Infrastructure;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;

namespace NimblePros.SampleToDo.Web.Configurations;

public static class MediatrConfig
{
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

    return services;
  }
}
