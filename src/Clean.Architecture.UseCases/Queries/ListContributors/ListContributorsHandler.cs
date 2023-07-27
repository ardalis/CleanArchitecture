using Ardalis.Result;
using MediatR;
using Clean.Architecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.UseCases.Queries.GetContributor;

public class ListContributorsHandler : IRequestHandler<ListContributorsCommand, Result<IEnumerable<ContributorDTO>>>
{
  private readonly AppDbContext _db;

  public ListContributorsHandler(AppDbContext db)
  {
    _db = db;
  }

  public async Task<Result<IEnumerable<ContributorDTO>>> Handle(ListContributorsCommand request, CancellationToken cancellationToken)
  {
    var result = await _db.Contributors.FromSqlRaw("SELECT Id, Name FROM Contributors") // don't fetch other big columns
      .Select(c => new ContributorDTO(c.Id, c.Name))
      .ToListAsync();

    return result;
  }
}
