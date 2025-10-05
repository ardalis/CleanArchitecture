using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.GetWithAllItems;

public record GetProjectWithAllItemsQuery(ProjectId ProjectId) : IQuery<Result<ProjectWithAllItemsDTO>>, ICacheable
{
  public string GetCacheKey()
  {
    return $"{nameof(GetProjectWithAllItemsQuery)}-{ProjectId}";
  }
}
