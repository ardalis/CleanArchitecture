namespace NimblePros.SampleToDo.Core.Interfaces;

public interface IHasDomainEvents
{
  IReadOnlyCollection<DomainEventBase> DomainEvents { get; }
  void ClearDomainEvents();
}
