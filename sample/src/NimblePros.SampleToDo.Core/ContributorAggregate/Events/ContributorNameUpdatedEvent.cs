namespace NimblePros.SampleToDo.Core.ContributorAggregate.Events;

internal sealed record ContributorNameUpdatedEvent(Contributor contributor) : DomainEventBase
{
  public Contributor Contributor { get; private set; } = contributor;
}
