using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDTO>>, ICacheable
{
  public string GetCacheKey()
  {
    return $"{nameof(GetContributorQuery)}-{ContributorId.Value}";
  }
}
