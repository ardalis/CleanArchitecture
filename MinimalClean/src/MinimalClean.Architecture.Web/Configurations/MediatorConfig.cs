namespace MinimalClean.Architecture.Web.Configurations;

public static class MediatorConfig
{
  // Should be called from ServiceConfigs.cs, not Program.cs
  public static IServiceCollection AddMediatorSourceGen(this IServiceCollection services,
    ILogger logger)
  {
    logger.LogInformation("Registering Mediator SourceGen and Behaviors");
    services.AddMediator(options =>
    {
      // Lifetime: Singleton is fastest per docs; Scoped/Transient also supported.
      options.ServiceLifetime = ServiceLifetime.Scoped;

      // Supply any TYPE from each assembly you want scanned (the generator finds the assembly from the type)
      options.Assemblies =
      [
        typeof(MediatorConfig)                  // Web
      ];

      // Register AOT pipeline behaviors here (order matters)
      options.PipelineBehaviors =
      [
        typeof(Nimble.Modulith.Web.LoggingBehavior<,>)
      ];

      // If you have stream behaviors:
      // options.StreamPipelineBehaviors = [ typeof(YourStreamBehavior<,>) ];
    });

    return services;
  }
}
