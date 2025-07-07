using Microsoft.Extensions.Configuration;
using NimblePros.SampleToDo.Core.Interfaces;
using NimblePros.SampleToDo.Infrastructure.Data;
using NimblePros.SampleToDo.Infrastructure.Data.Queries;
using NimblePros.SampleToDo.Infrastructure.Email;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.List;
using NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;
using NimblePros.SampleToDo.UseCases.Projects.ListShallow;
using NimblePros.Metronome;

namespace NimblePros.SampleToDo.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger,
    string environmentName)
  {
    if (environmentName == "Development")
    {
      RegisterDevelopmentOnlyDependencies(services, configuration);
    }
    else if (environmentName == "Testing")
    {
      RegisterTestingOnlyDependencies(services);
    }
    else
    {
      RegisterProductionOnlyDependencies(services, configuration);
    }
    
    RegisterEFRepositories(services);
    
    logger.LogInformation("{Project} services registered", "Infrastructure");
    
    return services;
  }

  private static void AddDbContextWithSqlite(IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("SqliteConnection");
    services.AddDbContext<AppDbContext>((provider, options) =>
              options.UseSqlite(connectionString)
              .AddMetronomeDbTracking(provider));
  }


  private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services, IConfiguration configuration)
  {
    AddDbContextWithSqlite(services, configuration);
    services.AddScoped<IEmailSender, SmtpEmailSender>();
    services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
    services.AddScoped<IListIncompleteItemsQueryService, ListIncompleteItemsQueryService>();
    services.AddScoped<IListProjectsShallowQueryService, ListProjectsShallowQueryService>();
  }

  private static void RegisterTestingOnlyDependencies(IServiceCollection services)
  {
    // do not configure a DbContext here for testing - it's configured in CustomWebApplicationFactory

    services.AddScoped<IEmailSender, FakeEmailSender>();
    services.AddScoped<IListContributorsQueryService, FakeListContributorsQueryService>();
    services.AddScoped<IListIncompleteItemsQueryService, FakeListIncompleteItemsQueryService>();
    services.AddScoped<IListProjectsShallowQueryService, FakeListProjectsShallowQueryService>();
  }

  private static void RegisterProductionOnlyDependencies(IServiceCollection services, IConfiguration configuration)
  {
    AddDbContextWithSqlite(services, configuration);

    services.AddScoped<IEmailSender, SmtpEmailSender>();
    services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
    services.AddScoped<IListIncompleteItemsQueryService, ListIncompleteItemsQueryService>();
    services.AddScoped<IListProjectsShallowQueryService, ListProjectsShallowQueryService>();
  }

  private static void RegisterEFRepositories(IServiceCollection services)
  {
    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
  }
}
