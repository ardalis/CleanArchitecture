using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Specifications;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CleanArchitecture.UnitTests.Core.Specifications
{
    public class IncompleteItemsSpecificationConstructor
    {
        [Fact]
        public void FilterCollectionToOnlyReturnItemsWithIsDoneFalse()
        {
            var item1 = new ToDoItem();
            var item2 = new ToDoItem();
            var item3 = new ToDoItem();
            item3.MarkComplete();

            var items = new List<ToDoItem>() { item1, item2, item3 };

            var spec = new IncompleteItemsSpecification();
            List<ToDoItem> filteredList = items
                .Where(spec.WhereExpressions.First().Compile())
                .ToList();

            Assert.Contains(item1, filteredList);
            Assert.Contains(item2, filteredList);
            Assert.DoesNotContain(item3, filteredList);
        }
    }
}
