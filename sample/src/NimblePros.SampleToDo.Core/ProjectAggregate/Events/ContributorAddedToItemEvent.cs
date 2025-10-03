namespace NimblePros.SampleToDo.Core.ProjectAggregate.Events;

public sealed record ContributorAddedToItemEvent : DomainEvent
{
  public int ContributorId { get; set; }
  public ToDoItem Item { get; set; }

  public ContributorAddedToItemEvent(ToDoItem item, int contributorId)
  {
    Item = item;
    ContributorId = contributorId;
  }
}
