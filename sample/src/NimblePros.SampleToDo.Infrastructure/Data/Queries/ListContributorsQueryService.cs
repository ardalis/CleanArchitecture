using NimblePros.SampleToDo.UseCases.Contributors;
using NimblePros.SampleToDo.UseCases.Contributors.List;
using Microsoft.EntityFrameworkCore;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class ListContributorsQueryService : IListContributorsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries
  private readonly AppDbContext _db;

  public ListContributorsQueryService(AppDbContext db)
  {
    _db = db;
  }

  public async Task<IEnumerable<ContributorDTO>> ListAsync()
  {
    var result = await _db.Contributors.FromSqlRaw("SELECT Id, Name FROM Contributors") // don't fetch other big columns
      .Select(c => new ContributorDTO(c.Id, c.Name))
      .ToListAsync();

    return result;
  }
}
