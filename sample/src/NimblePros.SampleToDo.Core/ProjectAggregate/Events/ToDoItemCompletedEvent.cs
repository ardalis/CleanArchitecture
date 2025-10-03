namespace NimblePros.SampleToDo.Core.ProjectAggregate.Events;

public sealed record ToDoItemCompletedEvent : DomainEvent
{
  public ToDoItem CompletedItem { get; set; }

  public ToDoItemCompletedEvent(ToDoItem completedItem)
  {
    CompletedItem = completedItem;
  }
}
