namespace NimblePros.SampleToDo.Core.Interfaces;
public interface IDomainEventDispatcher2
{
  Task DispatchAndClearEvents(IEnumerable<IAggregateRoot> aggregatesWithEvents);
}
