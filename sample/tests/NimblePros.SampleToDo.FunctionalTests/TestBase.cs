using NimblePros.SampleToDo.Infrastructure.Data;
using NimblePros.SampleToDo.Web;
using Microsoft.Extensions.DependencyInjection;

namespace NimblePros.SampleToDo.FunctionalTests;

/// <summary>
/// Base class for functional tests that provides database isolation between tests
/// </summary>
public abstract class TestBase : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
  protected readonly HttpClient _client;
  protected readonly CustomWebApplicationFactory<Program> _factory;

  protected TestBase(CustomWebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _client = factory.CreateClient();
  }

  public virtual Task InitializeAsync()
  {
    // Override in derived classes if needed for test-specific setup
    return Task.CompletedTask;
  }

  public virtual async Task DisposeAsync()
  {
    // Reset database to clean seed state after each test
    await ResetDatabaseAsync();
  }

  protected async Task ResetDatabaseAsync()
  {
    using var scope = _factory.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Clear all data in the correct order to handle foreign key relationships
    var toDoItems = dbContext.ToDoItems.ToList();
    var projects = dbContext.Projects.ToList();
    var contributors = dbContext.Contributors.ToList();
    
    dbContext.ToDoItems.RemoveRange(toDoItems);
    dbContext.Projects.RemoveRange(projects);
    dbContext.Contributors.RemoveRange(contributors);
    
    await dbContext.SaveChangesAsync();
    
    // Re-seed with fresh data
    await SeedData.PopulateTestDataAsync(dbContext);
  }
}