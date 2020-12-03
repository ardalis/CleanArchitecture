using Clean.Architecture.Core.Entities;
using Clean.Architecture.UnitTests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Clean.Architecture.IntegrationTests.Data
{
    public class EfRepositoryUpdate : BaseEfRepoTestFixture
    {
        [Fact]
        public async Task UpdatesItemAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ToDoItemBuilder().Title(initialTitle).Build();

            await repository.AddAsync(item);

            // detach the item so we get a different instance
            _dbContext.Entry(item).State = EntityState.Detached;

            // fetch the item and update its title
            var newItem = (await repository.ListAsync<ToDoItem>())
                .FirstOrDefault(i => i.Title == initialTitle);
            Assert.NotNull(newItem);
            Assert.NotSame(item, newItem);
            var newTitle = Guid.NewGuid().ToString();
            newItem.Title = newTitle;

            // Update the item
            await repository.UpdateAsync(newItem);
            var updatedItem = (await repository.ListAsync<ToDoItem>())
                .FirstOrDefault(i => i.Title == newTitle);

            Assert.NotNull(updatedItem);
            Assert.NotEqual(item.Title, updatedItem.Title);
            Assert.Equal(newItem.Id, updatedItem.Id);
        }
    }
}
