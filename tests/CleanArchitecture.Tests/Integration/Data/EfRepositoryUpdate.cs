using CleanArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace CleanArchitecture.Tests.Integration.Data
{
    public class EfRepositoryUpdate : BaseEfRepoTestFixture
    {
        [Fact]
        public void UpdatesItemAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ToDoItemBuilder().Title(initialTitle).Build();

            repository.Add(item);

            // detach the item so we get a different instance
            _dbContext.Entry(item).State = EntityState.Detached;

            // fetch the item and update its title
            var newItem = repository.List<ToDoItem>()
                .FirstOrDefault(i => i.Title == initialTitle);
            Assert.NotNull(newItem);
            Assert.NotSame(item, newItem);
            var newTitle = Guid.NewGuid().ToString();
            newItem.Title = newTitle;

            // Update the item
            repository.UpdateUsingOriginalMethod(newItem);
            var updatedItem = repository.List<ToDoItem>()
                .FirstOrDefault(i => i.Title == newTitle);

            Assert.NotNull(updatedItem);
            Assert.NotEqual(item.Title, updatedItem.Title);
            Assert.Equal(newItem.Id, updatedItem.Id);
        }
    }
}
