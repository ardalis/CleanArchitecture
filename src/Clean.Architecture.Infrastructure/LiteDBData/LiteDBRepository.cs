using Ardalis.Specification;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Infrastructure.LiteDBData;

public class LiteDBRepository<T> : IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  private readonly LiteDbContext _dbContext;

  public LiteDBRepository(LiteDbContext dbContext)
  {
    _dbContext = dbContext;
  } 

  public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
  {

    await _dbContext.Database.GetCollection<T>().InsertAsync(entity);
    return entity;
  }

  public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    // TODO: Figure out how to combine WhereExpressions
    return await _dbContext.Database.GetCollection<T>().CountAsync(specification.WhereExpressions.First());
  }

  public async Task<int> CountAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContext.Database.GetCollection<T>().CountAsync();
  }

  public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    await _dbContext.Database.GetCollection<T>().DeleteAsync(entity.Id);
  }

  public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
  {
    return await _dbContext.Database.GetCollection<T>()
      .FindByIdAsync(new LiteDB.BsonValue(id));
  }

  public Task<T?> GetBySpecAsync<Spec>(Spec specification, CancellationToken cancellationToken = default) where Spec : ISingleResultSpecification, ISpecification<T>
  {
    throw new NotImplementedException();
  }

  public Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
  {
    return (await _dbContext.Database.GetCollection<T>().FindAllAsync()).ToList();
  }

  public Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return Task.CompletedTask;
  }

  public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    await _dbContext.Database.GetCollection<T>()
      .UpdateAsync(entity);
  }
}
