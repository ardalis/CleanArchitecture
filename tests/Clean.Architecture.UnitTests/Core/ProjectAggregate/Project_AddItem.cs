using Clean.Architecture.Core.ProjectAggregate;
using System;
using Xunit;

namespace Clean.Architecture.UnitTests.Core.ProjectAggregate
{
    public class Project_AddItem
    {
        private Project _testProject = new Project("some name");

        [Fact]
        public void AddsItemToItems()
        {
            var _testItem = new ToDoItem
            {
                Title = "title",
                Description = "description"
            };

            _testProject.AddItem(_testItem);

            Assert.Contains(_testItem, _testProject.Items);
        }

        [Fact]
        public void ThrowsExceptionGivenNullItem()
        {
            Action action = () => _testProject.AddItem(null);

            var ex = Assert.Throws<ArgumentNullException>(action);
            Assert.Equal("newItem", ex.ParamName);
        }
    }
}
