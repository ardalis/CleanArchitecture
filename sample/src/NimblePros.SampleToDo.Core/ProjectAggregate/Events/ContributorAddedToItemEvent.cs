using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.Core.ProjectAggregate.Events;

public sealed class ContributorAddedToItemEvent(ToDoItem item, ContributorId contributorId) : DomainEventBase
{
  public ContributorId ContributorId { get; set; } = contributorId;
  public ToDoItem Item { get; set; } = item;

}
