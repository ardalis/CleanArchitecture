using Ardalis.Specification.EntityFrameworkCore;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : BaseEntity, IAggregateRoot
{
  private readonly AppDbContext _dbContext;

  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public IAsyncEnumerable<T> GetAsyncEnumerable()
  {
    return _dbContext.Set<T>().AsAsyncEnumerable<T>();
  }
}
