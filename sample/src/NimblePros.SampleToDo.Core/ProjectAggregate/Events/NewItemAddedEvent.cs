namespace NimblePros.SampleToDo.Core.ProjectAggregate.Events;

public sealed record NewItemAddedEvent(Project project, ToDoItem newItem) : DomainEvent;
