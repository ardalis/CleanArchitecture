using Vogen;

namespace NimblePros.SampleToDo.Core.ContributorAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ContributorName
{
  private static Validation Validate(in string name) => String.IsNullOrEmpty(name) ?
    Validation.Invalid("Name cannot be empty") :
    Validation.Ok;
}
