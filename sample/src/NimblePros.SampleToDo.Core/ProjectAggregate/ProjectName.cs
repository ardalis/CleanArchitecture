using Vogen;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial class ProjectName
{
  private static Validation Validate(in string name) => String.IsNullOrEmpty(name) ? 
    Validation.Invalid("Name cannot be empty") : 
    Validation.Ok;
}
