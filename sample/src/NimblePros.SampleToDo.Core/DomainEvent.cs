using NimblePros.SampleToDo.Core.Interfaces;

namespace NimblePros.SampleToDo.Core;

public abstract record DomainEventBase : IDomainEvent
{
  public DateTime OccurredUtc { get; init; } = DateTime.UtcNow;
  public string? CorrelationId { get; init; }
  public int Version { get; init; } = 1;
}
