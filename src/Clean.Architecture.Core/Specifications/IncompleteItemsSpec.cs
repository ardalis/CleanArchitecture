using Ardalis.Specification;
using Clean.Architecture.Core.ProjectAggregate;

namespace Clean.Architecture.Core.Specifications
{
    public class IncompleteItemsSpec : Specification<ToDoItem>
    {
        public IncompleteItemsSpec()
        {
            Query.Where(item => !item.IsDone);
        }
    }
}
