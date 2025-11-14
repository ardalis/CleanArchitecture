namespace MinimalClean.Architecture.Web.Domain.CartAggregate.Specifications;

public class CartByIdSpec : Specification<Cart>
{
  public CartByIdSpec(CartId cartId) =>
    Query
        .Include(c => c.Items)
        .Where(cart => cart.Id == cartId && !cart.Deleted);
}
