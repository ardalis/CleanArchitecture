using Vogen;

namespace MartiX.Clean.Architecture.Web.Domain.CartAggregate;

[ValueObject<Guid>]
public readonly partial struct CartItemId
{
  private static Validation Validate(Guid value)
      => value != Guid.Empty ? Validation.Ok : Validation.Invalid("CartItemId must set to non-default value.");
}
