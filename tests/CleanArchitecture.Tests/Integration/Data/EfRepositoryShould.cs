using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Core.Model;
using System.Linq;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Interfaces;
using Moq;

namespace CleanArchitecture.Tests.Integration.Data
{
    public class EfRepositoryAddShould
    {
        private static DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
        [Fact]
        public void AddItemAndSetId()
        {
            var repository = GetRepository();
            var item = new ToDoItem();

            repository.Add(item);

            var newItem = repository.List().FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem.Id > 0);
            
        }

        private EfRepository<ToDoItem> GetRepository()
        {
            var options = CreateNewContextOptions();
            var mockDispatcher = new Mock<IDomainEventDispatcher>();

            return new EfRepository<ToDoItem>(new AppDbContext(options, mockDispatcher.Object));
        }
    }
}