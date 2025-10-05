using NimblePros.SampleToDo.Core.ContributorAggregate.Events;

namespace NimblePros.SampleToDo.Core.ContributorAggregate;

public sealed class Contributor : EntityBase<Contributor, ContributorId>, IAggregateRoot
{
  public ContributorName Name { get; private set; }

  public Contributor(ContributorName name)
  {
      Name = name;
  }

  public Contributor UpdateName(ContributorName newName)
  {
    if (Name.Equals(newName)) return this;
    Name = newName;
    this.RegisterDomainEvent(new ContributorNameUpdatedEvent(this));
    return this;
  }
}

