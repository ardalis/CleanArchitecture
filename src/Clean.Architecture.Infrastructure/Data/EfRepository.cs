using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;

namespace Clean.Architecture.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T>(AppDbContext _dbContext) : 
  RepositoryBase<T>(_dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
}
