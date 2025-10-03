namespace NimblePros.SampleToDo.Core.ContributorAggregate.Events;

internal sealed record ContributorNameUpdatedEvent(Contributor contributor) : DomainEvent
{
  public Contributor Contributor { get; private set; } = contributor;
}
