namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.List;

public record ListContributorsQuery(int? Page = 1, int? PerPage = Constants.DEFAULT_PAGE_SIZE)
  : IQuery<Result<PagedResult<ContributorDto>>>, ICacheable
{
  public string? CacheProfile => "Long";

  public string GetCacheKey()
  {
    return $"{nameof(ListContributorsQuery)}-{Page}-{PerPage}";
  }
}
