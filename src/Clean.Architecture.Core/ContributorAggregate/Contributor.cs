using System.Security.Cryptography.X509Certificates;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using StronglyTypedIds;

namespace Clean.Architecture.Core.ContributorAggregate;

[StronglyTypedId(backingType:StronglyTypedIdBackingType.Int)]
public partial struct ContributorId  {}

public class Contributor(string name) : EntityBase<ContributorId>, IAggregateRoot
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
