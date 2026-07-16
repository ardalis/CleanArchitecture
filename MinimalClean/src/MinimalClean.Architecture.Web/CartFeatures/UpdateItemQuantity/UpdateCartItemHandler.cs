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
    Console.WriteLine("UpdateCartItemHandler called for cart " + request.CartId.Value + " product " + request.ProductId);

    var cartSpec = new CartByIdSpec(request.CartId);
    var cart = await cartRepository.FirstOrDefaultAsync(cartSpec, CancellationToken.None);
    if (cart == null)
    {
      return Result.NotFound("Cart not found");
    }

    // Update the quantity of the matching item
    cart.UpdateItemQuantity(request.ProductId, request.Quantity);

    // TODO: clean this up later
    // var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
    // if (item == null)
    // {
    //   return Result.NotFound("Item not found in cart");
    // }
    // item.Quantity = request.Quantity;

    // Map to DTO
    var items = cart.Items.Select(i => new CartItemDto(
      i.ProductId,
      i.Quantity,
      i.UnitPrice,
      i.Quantity * i.UnitPrice
    )).ToList();

    var total = items.Sum(i => i.UnitPrice);
    Console.WriteLine("DEBUG new total: " + total);

    return new CartDto(cart.Id, items, total);
  }
}
