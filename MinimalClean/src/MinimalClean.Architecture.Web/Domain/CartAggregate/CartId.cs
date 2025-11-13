using Vogen;

namespace MinimalClean.Architecture.Web.Domain.CartAggregate;

[ValueObject<Guid>]
public readonly partial struct CartId
{
  private static Validation Validate(Guid value)
      => value != Guid.Empty ? Validation.Ok : Validation.Invalid("CartId must set to non-default value.");
}
[ValueObject<int>]
public readonly partial struct TestId { }
