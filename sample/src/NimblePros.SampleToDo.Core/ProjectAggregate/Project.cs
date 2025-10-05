using NimblePros.SampleToDo.Core.ProjectAggregate.Events;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

public class Project : EntityBase<Project, ProjectId>, IAggregateRoot
{
  public ProjectName Name { get; private set; }

  private readonly List<ToDoItem> _items = [];
  public IEnumerable<ToDoItem> Items => _items.AsReadOnly();
  public ProjectStatus Status => _items.All(i => i.IsDone) ? ProjectStatus.Complete : ProjectStatus.InProgress;

  public Project(ProjectName name)
  {
    Name = name;
  }

  public Project AddItem(ToDoItem newItem)
  {
    Guard.Against.Null(newItem);
    _items.Add(newItem);

    var newItemAddedEvent = new NewItemAddedEvent(this, newItem);
    base.RegisterDomainEvent(newItemAddedEvent);
    return this;
  }

  public Project UpdateName(ProjectName newName)
  {
    Name = newName;
    return this;
  }
}
