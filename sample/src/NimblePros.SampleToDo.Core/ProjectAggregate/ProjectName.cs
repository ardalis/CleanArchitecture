using Vogen;

[assembly: VogenDefaults(
        staticAbstractsGeneration: StaticAbstractsGeneration.MostCommon | StaticAbstractsGeneration.InstanceMethodsAndProperties)]


namespace NimblePros.SampleToDo.Core.ProjectAggregate;

// NOTE: Structs do not require conversion to work with EF Core
[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ProjectName
{
  public const int MaxLength = 100;
  private static Validation Validate(in string name) =>
    string.IsNullOrEmpty(name)
      ? Validation.Invalid(ProjectErrorMessages.CoreProjectNameEmpty)
      : name.Length > MaxLength
        ? Validation.Invalid(ProjectErrorMessages.CoreProjectNameTooLong(MaxLength))
        : Validation.Ok;
}
