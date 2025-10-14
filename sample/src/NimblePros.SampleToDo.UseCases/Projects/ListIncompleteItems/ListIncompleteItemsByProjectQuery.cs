namespace NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;

public record ListIncompleteItemsByProjectQuery(int ProjectId) : 
  IQuery<Result<IEnumerable<ToDoItemDto>>>, ICacheable
{
  public string? CacheProfile => null;

  public string GetCacheKey()
  {
    return $"{nameof(ListIncompleteItemsByProjectQuery)}-{ProjectId}";
  }
}
