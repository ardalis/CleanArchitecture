using MinimalClean.Architecture.Web.Domain.CartAggregate;
using MinimalClean.Architecture.Web.Domain.CartAggregate.Specifications;

namespace MinimalClean.Architecture.Web.CartFeatures.UpdateItemQuantity;

public record UpdateCartItemCommand(CartId CartId, int ProductId, int Quantity) : ICommand<Result<CartDto>>;

public class UpdateCartItemHandler(
  IRepository<Cart> cartRepository)
  : ICommandHandler<UpdateCartItemCommand, Result<CartDto>>
{
  public async ValueTask<Result<CartDto>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
  {
    var cartSpec = new CartByIdSpec(request.CartId);
    var cart = await cartRepository.FirstOrDefaultAsync(cartSpec, CancellationToken.None);
    if (cart == null)
    {
      return Result.NotFound("Cart not found");
    }

    // Update the quantity of the matching item
    cart.UpdateItemQuantity(request.ProductId, request.Quantity);

    // Map to DTO
    var items = cart.Items.Select(i => new CartItemDto(
      i.ProductId,
      i.Quantity,
      i.UnitPrice,
      i.Quantity * i.UnitPrice
    )).ToList();

    var total = items.Sum(i => i.UnitPrice);

    return new CartDto(cart.Id, items, total);
  }
}
