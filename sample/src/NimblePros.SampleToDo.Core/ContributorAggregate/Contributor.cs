using NimblePros.SampleToDo.Core.ContributorAggregate.Events;

namespace NimblePros.SampleToDo.Core.ContributorAggregate;

public class Contributor : EntityBase, IAggregateRoot
{
  public ContributorName Name { get; private set; }

  public Contributor(ContributorName name)
  {
    Name = name;
  }

  public void UpdateName(ContributorName newName)
  {
    if (Name.Equals(newName)) return;
    Name = newName;
    this.RegisterDomainEvent(new ContributorNameUpdatedEvent(this));
  }
}

