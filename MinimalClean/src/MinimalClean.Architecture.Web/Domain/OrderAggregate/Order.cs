using MinimalClean.Architecture.Web.Domain.ProductAggregate;
using MinimalClean.Architecture.Web.Infrastructure.Data;

namespace MinimalClean.Architecture.Web.Domain.OrderAggregate;

public class Order : EntityBase<Order, OrderId>, IAggregateRoot
{
  private readonly List<OrderItem> _items = new();

  public Order(OrderId id, Guid guestUserId)
  {
    Id = id;
    GuestUserId = guestUserId;

  }

  public DateTimeOffset CreatedOn { get; private set; } = DateTimeOffset.UtcNow;
  public Guid GuestUserId { get; private set; }
  public DateTimeOffset? DatePaid { get; private set; }
  public string PaymentReference { get; private set; } = string.Empty;
  public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

  public decimal Total => _items.Sum(i => i.UnitPrice.Value * i.Quantity.Value);

  public void AddItem(ProductId productId, Quantity quantity, Price unitPrice)
  {
    var item = new OrderItem(Id, productId, quantity, unitPrice);

    if(Items.Any(i => i.ProductId == productId))
    {
      var existingItem = Items.First(i => i.ProductId == productId);
      existingItem.IncreaseQuantity(quantity);
      return;
    }
    _items.Add(item);
  }

  public void ConfirmPayment(DateTimeOffset datePaid, string paymentReference)
  {
    DatePaid = datePaid;
    PaymentReference = paymentReference;
  }
}
