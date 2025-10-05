using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate.Events;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

public class ToDoItem : EntityBase<ToDoItem, ToDoItemId>
{
  public ToDoItem() : this(Priority.Backlog)
  {
  }

  public ToDoItem(Priority priority)
  {
    Priority = priority;
  }

  public string Title { get; set; } = string.Empty; // TODO: Use Value Object
  public string Description { get; set; } = string.Empty; // TODO: Use Value Object
  public ContributorId? ContributorId { get; private set; } // tasks don't have anyone assigned when first created
  public bool IsDone { get; private set; }

  public Priority Priority { get; private set; }

  public ToDoItem MarkComplete()
  {
    if (!IsDone)
    {
      IsDone = true;

      RegisterDomainEvent(new ToDoItemCompletedEvent(this));
    }
    return this;
  }

  public ToDoItem AddContributor(ContributorId contributorId)
  {
    Guard.Against.Null(contributorId);
    ContributorId = contributorId;

    var contributorAddedToItem = new ContributorAddedToItemEvent(this, contributorId);
    base.RegisterDomainEvent(contributorAddedToItem);
    return this;
  }

  public ToDoItem RemoveContributor()
  {
    ContributorId = null;
    return this;
  }

  public override string ToString()
  {
    string status = IsDone ? "Done!" : "Not done.";
    return $"{Id}: Status: {status} - {Title} - Priority: {Priority}";
  }
}
