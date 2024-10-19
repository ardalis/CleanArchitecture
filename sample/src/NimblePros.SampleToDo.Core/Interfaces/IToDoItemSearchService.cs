using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.Core.Interfaces;

public interface IToDoItemSearchService
{
  Task<Result<ToDoItem>> GetNextIncompleteItemAsync(ProjectId projectId);
  Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(ProjectId projectId, string searchString);
}
