using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CleanArchitecture.IntegrationTests.Data
{
    public abstract class BaseEfRepoTestFixture
    {
        protected AppDbContext _dbContext;

        protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("cleanarchitecture")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        protected EfRepository GetRepository()
        {
            var options = CreateNewContextOptions();
            var mockDispatcher = new Mock<IDomainEventDispatcher>();

            _dbContext = new AppDbContext(options, mockDispatcher.Object);
            return new EfRepository(_dbContext);
        }


    }
}
