using Ardalis.Specification;

namespace Clean.Architecture.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : BaseEntity, IAggregateRoot
{
  IAsyncEnumerable<T> GetAsyncEnumerable();
}
