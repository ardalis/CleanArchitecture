namespace NimblePros.SampleToDo.Core.Interfaces;

public interface IDomainEvent : Mediator.INotification
{
  DateTime OccurredUtc { get; }
  string? CorrelationId { get; }
  int Version { get; }            // for event schema evolution
}
