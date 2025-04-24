namespace NimblePros.SampleToDo.Core.ContributorAggregate.Events;

internal class ContributorNameUpdatedEvent(Contributor contributor) : DomainEventBase
{
  public Contributor Contributor { get; private set; } = contributor;

}
