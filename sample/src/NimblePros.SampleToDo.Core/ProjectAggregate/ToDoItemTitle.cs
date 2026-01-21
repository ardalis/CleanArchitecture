using Vogen;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ToDoItemTitle
{
  public const int MaxLength = 100;
  private static Validation Validate(in string title) =>
    string.IsNullOrEmpty(title)
      ? Validation.Invalid(ProjectErrorMessages.CoreToDoItemTitleEmpty)
      : title.Length > MaxLength
        ? Validation.Invalid(ProjectErrorMessages.CoreToDoItemTitleTooLong(MaxLength))
        : Validation.Ok;
}
