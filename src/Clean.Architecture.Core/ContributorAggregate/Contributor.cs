using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Clean.Architecture.Core.ContributorAggregate;

public class Contributor : EntityBase, IAggregateRoot
{
  public string Name { get; private set; }
  public ContributorStatus Status { get; private set; } = ContributorStatus.NotSet;

  public Contributor(string name)
  {
    Name = Guard.Against.NullOrEmpty(name, nameof(name));
  }

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }
}
