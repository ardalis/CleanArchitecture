using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;

namespace MartiX.Clean.Architecture.Web.Tests;

public class ProductDomainTests
{
  [Test]
  public async Task UpdateNameAndPrice_WhenCalled_UpdatesProduct()
  {
    var product = new Product(ProductId.From(10), "Old", 3m);

    product.UpdateName("New").UpdatePrice(15m);

    await Assert.That(product.Name != "New").IsFalse();
    await Assert.That(product.UnitPrice != 15m).IsFalse();
  }

  [Test]
  public async Task Create_WhenCalled_AssignsPlaceholderProductId()
  {
    var product = Product.Create("Name", 10m);

    await Assert.That(product.Id.Value != 0).IsFalse();
  }
}

