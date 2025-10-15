using Vogen;

namespace Clean.Architecture.Core.ContributorAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ContributorName
{
  public const int MaxLength = 100;
  private static Validation Validate(in string name) =>
    string.IsNullOrEmpty(name)
      ? Validation.Invalid("Name cannot be empty")
      : name.Length > MaxLength
        ? Validation.Invalid($"Name cannot be longer than {MaxLength} characters")
        : Validation.Ok;
}
