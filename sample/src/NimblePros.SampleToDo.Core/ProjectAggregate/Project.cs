using NimblePros.SampleToDo.Core.ProjectAggregate.Events;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

public class Project : EntityBase, IAggregateRoot
{
  public string Name { get; private set; } = default!;

  private readonly List<ToDoItem> _items = [];
  public IEnumerable<ToDoItem> Items => _items.AsReadOnly();
  public ProjectStatus Status => _items.All(i => i.IsDone) ? ProjectStatus.Complete : ProjectStatus.InProgress;

  // Note: Probably it makes more sense to prioritize items, not projects, but this is just an example
  public Priority Priority { get; }

  public Project(string name, Priority priority)
  {
    UpdateName(name); // TODO: Replace with value object
    Priority = priority;
  }

  public Project AddItem(ToDoItem newItem)
  {
    Guard.Against.Null(newItem);
    _items.Add(newItem);

    var newItemAddedEvent = new NewItemAddedEvent(this, newItem);
    base.RegisterDomainEvent(newItemAddedEvent);
    return this;
  }

  public Project UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName);
    return this;
  }
}
