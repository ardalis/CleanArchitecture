namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.List;

public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDTO>>>,ICacheable
{
  public string GetCacheKey()
  {
    return $"{nameof(ListContributorsQuery)}-{Skip}-{Take}";
  }
}
