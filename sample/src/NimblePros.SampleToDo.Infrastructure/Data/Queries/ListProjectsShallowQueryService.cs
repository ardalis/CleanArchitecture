using Microsoft.EntityFrameworkCore;
using NimblePros.SampleToDo.UseCases.Projects.ListShallow;
using NimblePros.SampleToDo.UseCases.Projects;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class ListProjectsShallowQueryService : IListProjectsShallowQueryService
{
  private readonly AppDbContext _db;

  public ListProjectsShallowQueryService(AppDbContext db)
  {
    _db = db;
  }

  public async Task<IEnumerable<ProjectDTO>> ListAsync()
  {
    var result = await _db.Projects.FromSqlRaw("SELECT Id, Name FROM Projects") // don't fetch other big columns
      .Select(x => new ProjectDTO(x.Id, x.Name, x.Status.ToString()))
      .ToListAsync();

    return result;
  }
}
