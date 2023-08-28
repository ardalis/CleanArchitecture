using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

  public async Task<IEnumerable<ToDoItemDTO>> ListAsync(int projectId)
  {
    var projectParameter = new SqlParameter("@projectId", System.Data.SqlDbType.Int);
    var result = await _db.ToDoItems.FromSqlRaw("SELECT Id, Title, Description, IsDone, ContributorId FROM ToDoItems WHERE ProjectId = @ProjectId",
      projectParameter) // don't fetch other big columns
      .Select(x => new ToDoItemDTO(x.Id, x.Title, x.Description, x.IsDone, x.ContributorId))
      .ToListAsync();

    return result;
  }
}
