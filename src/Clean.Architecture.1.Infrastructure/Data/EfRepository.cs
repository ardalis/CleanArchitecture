using Ardalis.Specification.EntityFrameworkCore;
using Clean.Architecture._1.SharedKernel.Interfaces;

namespace Clean.Architecture._1.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
  }
}
