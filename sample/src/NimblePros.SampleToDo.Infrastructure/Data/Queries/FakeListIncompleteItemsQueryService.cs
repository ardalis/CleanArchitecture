using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects;
using NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class FakeListIncompleteItemsQueryService : IListIncompleteItemsQueryService
{
  public async Task<IEnumerable<ToDoItemDto>> ListAsync(int projectId)
  {
    var testItem = new ToDoItemDto(Id: ToDoItemId.From(1000), Title: "test", Description: "test description", IsComplete: false, null);
    return await Task.FromResult(new List<ToDoItemDto>() { testItem});
  }
}
