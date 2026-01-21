namespace NimblePros.SampleToDo.Core.ProjectAggregate.Specifications;

public class IncompleteItemsSearchSpec : Specification<ToDoItem>
{
  public IncompleteItemsSearchSpec(string searchString)
  {
    Query
        .Where(item => !item.IsDone &&
        (item.Title.Value.Contains(searchString) ||
        item.Description.Value.Contains(searchString)));
  }
}
