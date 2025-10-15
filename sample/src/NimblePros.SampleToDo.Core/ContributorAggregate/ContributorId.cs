using Vogen;

namespace NimblePros.SampleToDo.Core.ContributorAggregate;

[ValueObject<int>]
public partial struct ContributorId
{
    private static Validation Validate(int value)
        => value > 0 ? Validation.Ok : Validation.Invalid("ContributorId must be positive.");
}

