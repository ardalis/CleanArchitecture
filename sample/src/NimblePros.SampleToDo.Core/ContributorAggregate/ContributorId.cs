using Vogen;

namespace NimblePros.SampleToDo.Core.ContributorAggregate;

// Deserialization (values coming back from EF Core or other converters) skips Validate.
// EF Core gives new entities a temporary negative key until SaveChanges, and that value is
// converted to a ContributorId, so validating it would make every insert fail (see #1195).
// ContributorId.From() still validates, so IDs created in code must be positive.
[ValueObject<int>(deserializationStrictness: DeserializationStrictness.AllowAnything)]
public partial struct ContributorId
{
    private static Validation Validate(int value)
        => value > 0 ? Validation.Ok : Validation.Invalid("ContributorId must be positive.");
}

