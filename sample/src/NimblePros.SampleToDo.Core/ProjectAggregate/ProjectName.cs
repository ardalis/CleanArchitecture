using Vogen;

[assembly: VogenDefaults(
        staticAbstractsGeneration: StaticAbstractsGeneration.MostCommon | StaticAbstractsGeneration.InstanceMethodsAndProperties)]


namespace NimblePros.SampleToDo.Core.ProjectAggregate;

// NOTE: Structs do not require conversion to work with EF Core
[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ProjectName
{
  private static Validation Validate(in string name) => String.IsNullOrEmpty(name) ? 
    Validation.Invalid("Name cannot be empty") : 
    Validation.Ok;
}
