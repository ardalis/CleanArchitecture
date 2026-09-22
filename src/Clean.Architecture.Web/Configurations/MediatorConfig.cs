using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Infrastructure;
using Clean.Architecture.UseCases.Contributors.Create;

namespace Clean.Architecture.Web.Configurations;

public static partial class MediatorConfig
{
  // Should be called from ServiceConfigs.cs, not Program.cs.
  public static IServiceCollection AddMediatorSourceGen(
    this IServiceCollection services,
    Microsoft.Extensions.Logging.ILogger logger)
  {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentNullException.ThrowIfNull(logger);

    LogRegisteringMediator(logger);

    services.AddMediator(options =>
    {
      options.ServiceLifetime = ServiceLifetime.Scoped;

      options.Assemblies =
      [
        typeof(Contributor),
        typeof(CreateContributorCommand),
        typeof(InfrastructureServiceExtensions),
        typeof(MediatorConfig)
      ];

      options.PipelineBehaviors =
      [
        typeof(LoggingBehavior<,>)
      ];
    });

    return services;
  }

  [LoggerMessage(
    EventId = 1,
    Level = LogLevel.Information,
    Message = "Registering Mediator SourceGen and behaviors")]
  private static partial void LogRegisteringMediator(
    Microsoft.Extensions.Logging.ILogger logger);
}
