﻿using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Infrastructure;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;

namespace NimblePros.SampleToDo.Web.Configurations;

public static class MediatorConfig
{
  // Should be called from ServiceConfigs.cs, not Program.cs
  public static IServiceCollection AddMediatorSourceGen(this IServiceCollection services)
  {
    services.AddMediator(options =>
    {
      // Lifetime: Singleton is fastest per docs; Scoped/Transient also supported.
      options.ServiceLifetime = ServiceLifetime.Scoped;

      // Supply any TYPE from each assembly you want scanned (the generator finds the assembly from the type)
      options.Assemblies =
      [
        typeof(Contributor),                  // Core
        typeof(CreateContributorCommand),     // UseCases
        typeof(InfrastructureServiceExtensions) // Infrastructure
      ];

      // Register pipeline behaviors here (order matters)
      options.PipelineBehaviors =
      [
        typeof(LoggingBehavior<,>),
        typeof(CachingBehavior<,>)
      ];

      // If you have stream behaviors:
      // options.StreamPipelineBehaviors = [ typeof(YourStreamBehavior<,>) ];
    });

    // Alternative: register behaviors via DI yourself (useful if not doing AOT):
    // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

    return services;
  }
}
