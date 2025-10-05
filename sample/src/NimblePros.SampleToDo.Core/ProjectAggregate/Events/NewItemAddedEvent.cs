namespace NimblePros.SampleToDo.Core.ProjectAggregate.Events;

public sealed class NewItemAddedEvent(Project project, ToDoItem newItem) : DomainEventBase
{
  public Project Project { get; private set; } = project;
  public ToDoItem NewItem { get; private set; } = newItem;
}
