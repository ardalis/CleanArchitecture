using NimblePros.SampleToDo.UseCases;
using NimblePros.SampleToDo.UseCases.Contributors;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.List;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class ListContributorsQueryService : IListContributorsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries
  private readonly AppDbContext _db;

  public ListContributorsQueryService(AppDbContext db)
  {
    _db = db;
  }

  public async Task<PagedResult<ContributorDto>> ListAsync(int page, int perPage)
  {
    var items = await _db.Contributors.FromSqlRaw("SELECT Id, Name FROM Contributors") // don't fetch other big columns
      .OrderBy(c => c.Id)
      .Skip((page - 1) * perPage)
      .Take(perPage)
      .Select(c => new ContributorDto(c.Id, c.Name))
      .ToListAsync();

    int totalCount = await _db.Contributors.CountAsync();
    int totalPages = (int)Math.Ceiling(totalCount / (double)perPage);
    var result = new PagedResult<ContributorDto>(items, page, perPage, totalCount, totalPages);

    return result;
  }
}
