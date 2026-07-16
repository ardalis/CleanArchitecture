namespace MinimalClean.Architecture.Web.Domain.CartAggregate;

public class Cart : EntityBase<Cart, CartId>, IAggregateRoot
{
  private readonly List<CartItem> _items = new();

  public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
  public bool Deleted { get; private set; } = false;
  public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

  public void AddItem(int productId, int quantity, decimal unitPrice)
  {
    var item = new CartItem(productId, quantity, unitPrice);
    _items.Add(item);
  }

  public void UpdateItemQuantity(int productId, int quantity)
  {
    var item = _items.FirstOrDefault(i => i.ProductId == productId);
    if (item != null)
    {
      item.Quantity = quantity;
    }
  }

    public void MarkAsDeleted() => Deleted = true;
}
