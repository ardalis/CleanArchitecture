using NimblePros.SampleToDo.Core;

namespace NimblePros.SampleToDo.Infrastructure.Data;
public interface IDomainEventDispatcher2
{
  Task DispatchAndClearEvents(IEnumerable<AggregateRoot> aggregatesWithEvents);
}