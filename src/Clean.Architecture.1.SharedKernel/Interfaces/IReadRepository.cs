using Ardalis.Specification;

namespace Clean.Architecture._1.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
