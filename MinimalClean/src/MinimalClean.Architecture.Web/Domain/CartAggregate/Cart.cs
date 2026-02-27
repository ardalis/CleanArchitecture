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

    public void MarkAsDeleted() => Deleted = true;
}
