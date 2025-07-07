namespace Clean.Architecture.Core.ContributorAggregate;

public class Contributor : EntityBase, IAggregateRoot
{
  public Contributor(string name)
  {
    UpdateName(name); // TODO: Replace with value object and use primary constructor to populate field.
  }
  public string Name { get; private set; } = default!;
  public ContributorStatus Status { get; private set; } = ContributorStatus.NotSet;
  public PhoneNumber? PhoneNumber { get; private set; }
  public Contributor SetPhoneNumber(string phoneNumber)
  {
    PhoneNumber = new PhoneNumber(string.Empty, phoneNumber, string.Empty);
    return this;
  }

  public Contributor UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
    return this;
  }
}

public class PhoneNumber(string countryCode, string number, string? extension) : ValueObject
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
