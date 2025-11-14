using Vogen;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ToDoItemDescription
{
  public const int MaxLength = 200;
  private static Validation Validate(in string description) =>
    string.IsNullOrEmpty(description)
      ? Validation.Invalid("Description cannot be empty")
      : description.Length > MaxLength
        ? Validation.Invalid($"Description cannot be longer than {MaxLength} characters")
        : Validation.Ok;
}
