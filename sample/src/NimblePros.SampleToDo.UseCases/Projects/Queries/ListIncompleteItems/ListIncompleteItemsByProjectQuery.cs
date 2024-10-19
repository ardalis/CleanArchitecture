namespace NimblePros.SampleToDo.UseCases.Projects.Queries.ListIncompleteItems;

public record ListIncompleteItemsByProjectQuery(int ProjectId) : IQuery<Result<IEnumerable<ToDoItemDTO>>>;
