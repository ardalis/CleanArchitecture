using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.GetById;

public record GetContributorByIdQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>, ICacheable
{
  public string? CacheProfile => null;

  public string GetCacheKey()
  {
    return $"{nameof(GetContributorByIdQuery)}-{ContributorId.Value}";
  }
}
