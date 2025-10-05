namespace NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListIncompleteItemsQueryService
{
  Task<IEnumerable<ToDoItemDto>> ListAsync(int projectId);
}
