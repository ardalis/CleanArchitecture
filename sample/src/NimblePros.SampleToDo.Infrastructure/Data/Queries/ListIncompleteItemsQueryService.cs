using NimblePros.SampleToDo.UseCases.Projects;
using NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class ListIncompleteItemsQueryService : IListIncompleteItemsQueryService
{
  private readonly AppDbContext _db;

  public ListIncompleteItemsQueryService(AppDbContext db)
  {
    _db = db;
  }

  public async Task<IEnumerable<ToDoItemDto>> ListAsync(int projectId)
  {
    // FromSql turns the interpolated projectId into a DbParameter for whichever provider
    // is configured (SQLite or SQL Server), so the query is parameterized and portable.
    // Only the columns needed for the DTO are selected (don't fetch other big columns).
    var result = await _db.ToDoItems.FromSql(
        $"SELECT Id, Title, Description, IsDone, ContributorId FROM ToDoItems WHERE ProjectId = {projectId} AND IsDone = 0")
      .Select(x => new ToDoItemDto(x.Id, x.Title.Value, x.Description.Value, x.IsDone, x.ContributorId))
      .ToListAsync();

    return result;
  }
}
