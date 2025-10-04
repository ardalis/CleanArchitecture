namespace NimblePros.SampleToDo.Core.Interfaces;

/// <summary>
/// An abstraction for read only persistence operations, based on Ardalis.Specification.
/// Use this primarily to fetch trackable domain entities, not for custom queries.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
