using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Infrastructure.Data;

namespace Clean.Architecture.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture : IDisposable, IAsyncDisposable
{
  private readonly AppDbContext _dbContext;

  protected AppDbContext DbContext => _dbContext;

  protected BaseEfRepoTestFixture()
  {
    var options = CreateNewContextOptions();
    _dbContext = new AppDbContext(options);
  }

  protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
  {
    var fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .AddScoped<IDomainEventDispatcher>(_ => fakeEventDispatcher)
        .AddScoped<EventDispatchInterceptor>()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var interceptor = serviceProvider.GetRequiredService<EventDispatchInterceptor>();

    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseInMemoryDatabase("cleanarchitecture")
           .UseInternalServiceProvider(serviceProvider)
           .AddInterceptors(interceptor);

    return builder.Options;
  }

  protected EfRepository<Contributor> GetRepository()
  {
    return new EfRepository<Contributor>(_dbContext);
  }

  public void Dispose()
  {
    GC.SuppressFinalize(this);
    _dbContext.Dispose();
  }

  public async ValueTask DisposeAsync()
  {
    GC.SuppressFinalize(this);
    await _dbContext.DisposeAsync();
  }
}
