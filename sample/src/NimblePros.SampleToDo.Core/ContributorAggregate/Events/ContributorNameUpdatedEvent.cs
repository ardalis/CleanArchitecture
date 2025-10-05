namespace NimblePros.SampleToDo.Core.ContributorAggregate.Events;

public sealed class ContributorNameUpdatedEvent(Contributor contributor) : DomainEventBase
{
  public Contributor Contributor { get; private set; } = contributor;
}
