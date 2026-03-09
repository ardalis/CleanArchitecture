using MartiX.Clean.Architecture.Web.Domain.OrderAggregate;
using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;
using DomainOrder = MartiX.Clean.Architecture.Web.Domain.OrderAggregate.Order;

namespace MartiX.Clean.Architecture.Web.Tests;

public class OrderDomainTests
{
  [Test]
  public async Task AddItem_WhenDuplicateProduct_MergesQuantityAndUpdatesTotal()
  {
    var order = new DomainOrder(OrderId.From(Guid.NewGuid()), Guid.NewGuid());
    var productId = ProductId.From(7);

    order.AddItem(productId, Quantity.From(2), Price.From(5m));
    order.AddItem(productId, Quantity.From(3), Price.From(5m));
    order.ConfirmPayment(DateTimeOffset.UtcNow, "ref-123");

    await Assert.That(order.Items.Count != 1).IsFalse();
    await Assert.That(order.Items[0].Quantity.Value != 5).IsFalse();
    await Assert.That(order.Total != 25m).IsFalse();
    await Assert.That(order.PaymentReference != "ref-123").IsFalse();
  }
}

