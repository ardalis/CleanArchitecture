using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.GetWithAllItems;

public record GetProjectWithAllItemsQuery(ProjectId ProjectId) : 
  IQuery<Result<ProjectWithAllItemsDto>>, ICacheable
{
  public string? CacheProfile => "Short";

  public string GetCacheKey()
  {
    return $"{nameof(GetProjectWithAllItemsQuery)}-{ProjectId}";
  }
}
