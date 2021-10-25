using Ardalis.Specification;

namespace Clean.Architecture.Core.ProjectAggregate.Specifications;

public class IncompleteItemsSearchSpec : Specification<ToDoItem>
{
    public IncompleteItemsSearchSpec(string searchString)
    {
        Query
            .Where(item => !item.IsDone &&
            (item.Title.Contains(searchString) ||
            item.Description.Contains(searchString)));
    }
}
