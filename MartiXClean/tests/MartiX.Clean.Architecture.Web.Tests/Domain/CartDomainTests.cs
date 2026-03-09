using MartiX.Clean.Architecture.Web.Domain.CartAggregate;

namespace MartiX.Clean.Architecture.Web.Tests;

public class CartDomainTests
{
  [Test]
  public async Task AddItem_WhenCalled_StoresItemAndMarksDeleted()
  {
    var cart = new Cart();

    cart.AddItem(productId: 3, quantity: 2, unitPrice: 9.5m);
    cart.MarkAsDeleted();

    await Assert.That(cart.Items.Count != 1).IsFalse();
    await Assert.That(cart.Deleted).IsTrue();
    await Assert.That(cart.Items[0].ProductId != 3).IsFalse();
  }
}

