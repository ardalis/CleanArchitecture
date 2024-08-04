using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Clean.Architecture.Core.ContributorAggregate;

public class Contributor(string name) : EntityBase, IAggregateRoot
{
  // Example of validating primary constructor inputs
  // See: https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/primary-constructors#initialize-base-class
  public string Name { get; private set; } = Guard.Against.NullOrEmpty(name, nameof(name));
  public ContributorStatus Status { get; private set; } = ContributorStatus.NotSet;
  public PhoneNumber? PhoneNumber { get; private set; }

  public void SetPhoneNumber(string phoneNumber) => PhoneNumber = new PhoneNumber(string.Empty, phoneNumber, string.Empty);

  public void UpdateName(string newName) => Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
}

public class PhoneNumber(string countryCode,
  string number,
  string? extension) : ValueObject
{
  public string CountryCode { get; private set; } = countryCode;
  public string Number { get; private set; } = number;
  public string? Extension { get; private set; } = extension;

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return CountryCode;
    yield return Number;
    yield return Extension ?? String.Empty;
  }
}
