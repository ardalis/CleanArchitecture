using Mediator;
using NimblePros.SampleToDo.Core;
using NimblePros.SampleToDo.Core.Interfaces;

namespace NimblePros.SampleToDo.Infrastructure.Data;

public class MediatorDomainEventDispatcher : IDomainEventDispatcher2
{
  private readonly IMediator _mediator;

  private readonly ILogger<MediatRDomainEventDispatcher> _logger;

  public MediatorDomainEventDispatcher(IMediator mediator, ILogger<MediatRDomainEventDispatcher> logger)
  {
    _mediator = mediator;
    _logger = logger;
  }

  public async Task DispatchAndClearEvents(IEnumerable<AggregateRoot> aggregatesWithEvents)
  {
    var eventsToPublish = aggregatesWithEvents.SelectMany(a => a.DomainEvents).ToArray();

    foreach (var aggregate in aggregatesWithEvents)
    {
      aggregate.ClearDomainEvents();
    }

    foreach (DomainEventBase domainEvent in eventsToPublish)
    {
      await _mediator.Publish(domainEvent).ConfigureAwait(continueOnCapturedContext: false);
    }
  }
}

