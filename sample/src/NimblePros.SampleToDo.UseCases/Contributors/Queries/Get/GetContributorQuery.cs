using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>, ICacheable
{
  public string? CacheProfile => null;

  public string GetCacheKey()
  {
    return $"{nameof(GetContributorQuery)}-{ContributorId.Value}";
  }
}
