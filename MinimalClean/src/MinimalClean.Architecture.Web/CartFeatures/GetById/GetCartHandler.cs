using MinimalClean.Architecture.Web.Domain.CartAggregate;
using MinimalClean.Architecture.Web.Domain.CartAggregate.Specifications;

namespace MinimalClean.Architecture.Web.CartFeatures.GetById;

public record GetCartQuery(CartId CartId) : IQuery<Result<CartDto>>;
public class GetCartHandler(IReadRepository<Domain.CartAggregate.Cart> repository)
  : IQueryHandler<GetCartQuery, Result<CartDto>>
{
  public async ValueTask<Result<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
  {
    var spec = new CartByIdSpec(request.CartId);
    var cart = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (cart == null)
    {
      return Result.NotFound("Cart not found");
    }

    // Map to DTO
    var items = cart.Items.Select(i => new CartItemDto(
      i.ProductId,
      i.Quantity,
      i.UnitPrice,
      i.Quantity * i.UnitPrice
    )).ToList();

    var total = items.Sum(i => i.TotalPrice);

    return new CartDto(cart.Id, items, total);
  }
}
