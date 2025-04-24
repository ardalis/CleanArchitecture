namespace NimblePros.SampleToDo.Core.ContributorAggregate.Events;

/// <summary>
/// A domain event that is dispatched whenever a contributor is deleted.
/// The DeleteContributorService is used to dispatch this event.
/// </summary>
internal class ContributorDeletedEvent(int contributorId) : DomainEventBase
{
  public int ContributorId { get; set; } = contributorId;
}
