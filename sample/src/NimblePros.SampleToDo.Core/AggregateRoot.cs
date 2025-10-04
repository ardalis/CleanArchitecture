using NimblePros.SampleToDo.Core.Interfaces;

namespace NimblePros.SampleToDo.Core;
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : struct
{
  private List<IDomainEvent> _domainEvents = new();
  public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

  IReadOnlyCollection<DomainEventBase> IHasDomainEvents.DomainEvents => throw new NotImplementedException();

  public void ClearDomainEvents() => _domainEvents.Clear();
  protected void RegisterDomainEvent(IDomainEvent domainEvent)
  {
    _domainEvents.Add(domainEvent);
  }
}
