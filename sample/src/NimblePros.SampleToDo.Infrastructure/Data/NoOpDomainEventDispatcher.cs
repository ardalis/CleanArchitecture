namespace NimblePros.SampleToDo.Infrastructure.Data;

class NoOpDomainEventDispatcher : IDomainEventDispatcher
{
  public async Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents> entitiesWithEvents)
  {
    await Task.CompletedTask;
  }
}
