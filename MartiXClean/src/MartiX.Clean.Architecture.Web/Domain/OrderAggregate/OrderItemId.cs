using Vogen;

namespace MartiX.Clean.Architecture.Web.Domain.OrderAggregate;

[ValueObject<Guid>]
public readonly partial struct OrderItemId
{
  private static Validation Validate(Guid value)
      => value != Guid.Empty ? Validation.Ok : Validation.Invalid("OrderItemId cannot be empty.");
}
