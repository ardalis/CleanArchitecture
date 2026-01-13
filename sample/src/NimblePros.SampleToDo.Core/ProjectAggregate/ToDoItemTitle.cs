using Vogen;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ToDoItemTitle
{
  public const int MaxLength = 100;
  private static Validation Validate(in string title) =>
    string.IsNullOrEmpty(title)
      ? Validation.Invalid("Title cannot be empty")
      : title.Length > MaxLength
        ? Validation.Invalid($"Title cannot be longer than {MaxLength} characters")
        : Validation.Ok;
}
