namespace NimblePros.SampleToDo.Core.ProjectAggregate.Events;

public sealed class ToDoItemCompletedEvent(ToDoItem completedItem) : DomainEventBase
{
  public ToDoItem CompletedItem { get; private set; } = completedItem;
}
