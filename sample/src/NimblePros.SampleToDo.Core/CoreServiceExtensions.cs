using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NimblePros.SampleToDo.Core.Interfaces;
using NimblePros.SampleToDo.Core.Services;

namespace NimblePros.SampleToDo.Core;

public static class CoreServiceExtensions
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services, ILogger logger)
  {
    services.AddScoped<IToDoItemSearchService, ToDoItemSearchService>();
    services.AddScoped<IDeleteContributorService, DeleteContributorService>();
    
    logger.LogInformation("{Project} services registered", "Core");

    return services;
  }
}
