using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Infrastructure.Data;

namespace Clean.Architecture.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture : IDisposable
{
  private readonly ServiceProvider _serviceProvider;
  private readonly AppDbContext _dbContext;
  private bool _disposed;

  protected AppDbContext TestDbContext => _dbContext;

  protected BaseEfRepoTestFixture()
  {
    var fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();

    _serviceProvider = new ServiceCollection()
      .AddEntityFrameworkInMemoryDatabase()
      .AddScoped<IDomainEventDispatcher>(_ => fakeEventDispatcher)
      .AddScoped<EventDispatchInterceptor>()
      .BuildServiceProvider();

    var interceptor =
      _serviceProvider.GetRequiredService<EventDispatchInterceptor>();

    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase("cleanarchitecture")
      .UseInternalServiceProvider(_serviceProvider)
      .AddInterceptors(interceptor)
      .Options;

    _dbContext = new AppDbContext(options);
  }

  public void Dispose()
  {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (_disposed)
    {
      return;
    }

    if (disposing)
    {
      _dbContext.Dispose();
      _serviceProvider.Dispose();
    }

    _disposed = true;
  }

  // This is intentionally a factory method because every call creates
  // a new repository instance.
#pragma warning disable CA1024
  protected EfRepository<Contributor> GetRepository()
#pragma warning restore CA1024
  {
    return new EfRepository<Contributor>(_dbContext);
  }
}
