using Ardalis.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NimblePros.SampleToDo.Core.Interfaces;
using NimblePros.SampleToDo.Infrastructure.Data;
using NimblePros.SampleToDo.Infrastructure.Data.Queries;
using NimblePros.SampleToDo.Infrastructure.Email;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.List;
using NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;
using NimblePros.SampleToDo.UseCases.Projects.ListShallow;

namespace NimblePros.SampleToDo.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ILogger logger,
    bool isDevelopment)
  {
    if (isDevelopment)
    {
      RegisterDevelopmentOnlyDependencies(services);
    }
    else
    {
      RegisterProductionOnlyDependencies(services);
    }
    
    RegisterEF(services);
    
    logger.LogInformation("{Project} services registered", "Infrastructure");
    
    return services;
  }

  private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services)
  {
    services.AddScoped<IEmailSender, SmtpEmailSender>();
    services.AddScoped<IListContributorsQueryService, FakeListContributorsQueryService>();
    services.AddScoped<IListIncompleteItemsQueryService, FakeListIncompleteItemsQueryService>();
    services.AddScoped<IListProjectsShallowQueryService, FakeListProjectsShallowQueryService>();
  }
  
  private static void RegisterProductionOnlyDependencies(IServiceCollection services)
  {
    services.AddScoped<IEmailSender, SmtpEmailSender>();
    services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
    services.AddScoped<IListIncompleteItemsQueryService, ListIncompleteItemsQueryService>();
    services.AddScoped<IListProjectsShallowQueryService, ListProjectsShallowQueryService>();
  }

  private static void RegisterEF(IServiceCollection services)
  {
    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
  }
}
