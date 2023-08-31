using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;

public record ListIncompleteItemsByProjectQuery(int ProjectId) : IQuery<Result<IEnumerable<ToDoItemDTO>>>;
