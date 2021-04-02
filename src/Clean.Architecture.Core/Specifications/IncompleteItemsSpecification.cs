using Ardalis.Specification;
using Clean.Architecture.Core.ProjectAggregate;

namespace Clean.Architecture.Core.Specifications
{
    public class IncompleteItemsSpecification : Specification<ToDoItem>
    {
        public IncompleteItemsSpecification()
        {
            Query.Where(item => !item.IsDone);
        }
    }
}
