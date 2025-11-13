using Vogen;

namespace MinimalClean.Architecture.Web.Domain.OrderAggregate;

[ValueObject<decimal>]
public readonly partial struct Price
{
  private static Validation Validate(decimal value)
      => value > 0 ? Validation.Ok : Validation.Invalid("Price must be greater than zero.");
}
