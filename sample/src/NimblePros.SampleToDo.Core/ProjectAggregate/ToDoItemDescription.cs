using Vogen;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ToDoItemDescription
{
  public const int MaxLength = 200;
  private static Validation Validate(in string description) =>
    string.IsNullOrEmpty(description)
      ? Validation.Invalid(ProjectErrorMessages.CoreToDoItemDescriptionEmpty)
      : description.Length > MaxLength
        ? Validation.Invalid(ProjectErrorMessages.CoreToDoItemDescriptionTooLong(MaxLength))
        : Validation.Ok;
}
