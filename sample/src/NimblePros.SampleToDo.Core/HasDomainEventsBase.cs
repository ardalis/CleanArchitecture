using System.ComponentModel.DataAnnotations.Schema;
using NimblePros.SampleToDo.Core.Interfaces;

namespace NimblePros.SampleToDo.Core;

public abstract class HasDomainEventsBase : IHasDomainEvents
{
  private readonly List<DomainEventBase> _domainEvents = new();
  [NotMapped]
  public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  public void ClearDomainEvents() => _domainEvents.Clear();
}

