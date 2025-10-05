namespace NimblePros.SampleToDo.Core.ContributorAggregate.Events;

/// <summary>
/// A domain event that is dispatched whenever a contributor is deleted.
/// The DeleteContributorService is used to dispatch this event.
/// </summary>
public sealed class ContributorDeletedEvent(ContributorId contributorId) : DomainEventBase
{
  public ContributorId ContributorId { get; private set; } = contributorId;
}
