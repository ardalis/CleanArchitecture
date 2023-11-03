using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Clean.Architecture.Core.ContributorAggregate;

public class Contributor(string name) : EntityBase, IAggregateRoot
{
  // Example of validating primary constructor inputs
  // See: https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/primary-constructors#initialize-base-class
  public string Name { get; private set; } = Guard.Against.NullOrEmpty(name, nameof(name));
  public ContributorStatus Status { get; private set; } = ContributorStatus.NotSet;

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }
}
