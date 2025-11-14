namespace Clean.Architecture.Core.ContributorAggregate;

public class PhoneNumber(string countryCode, string number, string? extension) : ValueObject
{
  public static PhoneNumber Unknown { get; } = new PhoneNumber(String.Empty, String.Empty, String.Empty);
  public string CountryCode { get; private set; } = countryCode;
  public string Number { get; private set; } = number;
  public string? Extension { get; private set; } = extension;

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return CountryCode;
    yield return Number;
    yield return Extension ?? String.Empty;
  }

  public override string ToString()
  {
    return $"{CountryCode} {Number}{(string.IsNullOrEmpty(Extension) ? String.Empty : $" x{Extension}")}";
  }
}
