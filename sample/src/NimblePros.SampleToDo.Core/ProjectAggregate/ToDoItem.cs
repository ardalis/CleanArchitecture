using System.Xml.Linq;
using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate.Events;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

public class ToDoItem : EntityBase<ToDoItem, ToDoItemId>
{
  public ToDoItem() : this(Priority.Backlog)
  {
  }
  public ToDoItem(ToDoItemTitle title, ToDoItemDescription description) : this(Priority.Backlog)
  {
    Title = title;
    Description = description;
  }

  public ToDoItem(Priority priority)
  {
    Priority = priority;
  }

  public ToDoItemTitle Title { get; private set; }
  public ToDoItemDescription Description { get; private set; }
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

  public ToDoItem UpdateTitle(ToDoItemTitle newTitle)
  {
    if (Title.Equals(newTitle)) return this;
    Title = newTitle;
    return this;
  }

  public ToDoItem UpdateDescription(ToDoItemDescription newDescription)
  {
    if (Description.Equals(newDescription)) return this;
    Description = newDescription;
    return this;
  }

  public override string ToString()
  {
    string status = IsDone ? "Done!" : "Not done.";
    return $"{Id}: Status: {status} - {Title.Value} - Priority: {Priority}";
  }
}
