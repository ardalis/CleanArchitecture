namespace MinimalClean.Architecture.Web.Domain.CartAggregate;

public class CartItem : EntityBase<CartItem, CartItemId>
{
  // Private constructor for EF Core
  private CartItem() { }
  
  public CartItem(int productId, int quantity, decimal unitPrice)
  {
    ProductId = productId;
    Quantity = quantity;
    UnitPrice = unitPrice;
  }

  public int ProductId { get; private set; }
  public int Quantity { get; private set; }
  public decimal UnitPrice { get; private set; }
}
