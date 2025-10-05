using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases;
using NimblePros.SampleToDo.UseCases.Contributors;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.List;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class FakeListContributorsQueryService : IListContributorsQueryService
{
  public Task<PagedResult<ContributorDto>> ListAsync(int page, int perPage)
  {
    var items = new List<ContributorDto>();
    for (int i = 1; i <= 25; i++)
    { 
      items.Add(new ContributorDto(ContributorId.From(i), ContributorName.From($"Fake {i}")));
    };

    int totalPages = (int)Math.Ceiling(items.Count / (double)perPage);
    var result = new PagedResult<ContributorDto>(items, page, perPage, items.Count, totalPages);
    return Task.FromResult(result);
  }
}
