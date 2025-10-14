namespace NimblePros.SampleToDo.UseCases.Projects.ListShallow;

public record ListProjectsShallowQuery(int? Skip, int? Take) :
  IQuery<Result<IEnumerable<ProjectDto>>>, ICacheable
{
  public string? CacheProfile => null;

  public string GetCacheKey()
  {
    return $"{nameof(ListProjectsShallowQuery)}-{Skip}-{Take}";
  }
}
