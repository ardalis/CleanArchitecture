using Microsoft.Data.Sqlite;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.Infrastructure.Data;
using NimblePros.SampleToDo.Infrastructure.Data.Queries;

namespace NimblePros.SampleToDo.IntegrationTests.Data;

/// <summary>
/// ListIncompleteItemsQueryService uses raw SQL, which the EF Core InMemory provider
/// can't run, and the functional tests substitute a fake. Run it against SQLite.
/// </summary>
public sealed class ListIncompleteItemsQueryServiceTests : IDisposable
{
  private readonly SqliteConnection _connection = new("DataSource=:memory:");
  private readonly DbContextOptions<AppDbContext> _options;

  public ListIncompleteItemsQueryServiceTests()
  {
    _connection.Open();
    _options = new DbContextOptionsBuilder<AppDbContext>()
      .UseSqlite(_connection)
      .Options;

    using var db = new AppDbContext(_options);
    db.Database.EnsureCreated();
  }

  [Fact]
  public async Task ReturnsOnlyIncompleteItemsForTheRequestedProject()
  {
    var cancellationToken = TestContext.Current.CancellationToken;
    int projectId;

    await using (var db = new AppDbContext(_options))
    {
      var project = new Project(ProjectName.From("Project A"))
        .AddItem(NewItem("Open item 1"))
        .AddItem(NewItem("Open item 2"))
        .AddItem(NewItem("Done item").MarkComplete());

      var otherProject = new Project(ProjectName.From("Project B"))
        .AddItem(NewItem("Other project item"));

      db.Projects.AddRange(project, otherProject);
      await db.SaveChangesAsync(cancellationToken);
      projectId = project.Id.Value;
    }

    await using var queryDb = new AppDbContext(_options);
    var service = new ListIncompleteItemsQueryService(queryDb);

    var items = (await service.ListAsync(projectId)).ToList();

    Assert.Equal(["Open item 1", "Open item 2"], items.Select(i => i.Title).Order());
    Assert.All(items, i => Assert.False(i.IsComplete));
  }

  private static ToDoItem NewItem(string title) =>
    new(ToDoItemTitle.From(title), ToDoItemDescription.From($"{title} description"));

  public void Dispose() => _connection.Dispose();
}
