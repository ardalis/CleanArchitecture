using Microsoft.EntityFrameworkCore;
using MartiX.Clean.Architecture.Web.Domain.CartAggregate;
using MartiX.Clean.Architecture.Web.Domain.GuestUserAggregate;
using MartiX.Clean.Architecture.Web.Domain.OrderAggregate;
using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;
using MartiX.Clean.Architecture.Web.Feature.Cart.AddToCart;
using MartiX.Clean.Architecture.Web.Feature.Cart.Checkout;
using MartiX.Clean.Architecture.Web.Feature.Cart.GetById;
using MartiX.Clean.Architecture.Web.Infrastructure.Data;
using MartiX.WebApi.Results;
using DomainOrder = MartiX.Clean.Architecture.Web.Domain.OrderAggregate.Order;

namespace MartiX.Clean.Architecture.Web.Tests;

public class CartHandlersTests
{
  [Test]
  public async Task Handle_WhenProductMissing_ReturnsNotFound()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var cartRepo = new EfRepository<Cart>(db);
    var productRepo = new EfRepository<Product>(db);
    var handler = new AddToCartHandler(cartRepo, productRepo);

    var result = await handler.Handle(new AddToCartCommand(null, ProductId: 1, Quantity: 1), CancellationToken.None);

    await Assert.That(result.Status != ResultStatus.NotFound).IsFalse();
  }

  [Test]
  public async Task Handle_WhenProductExists_CreatesCartAndMapsTotal()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var cartRepo = new EfRepository<Cart>(db);
    var productRepo = new EfRepository<Product>(db);
    await productRepo.AddAsync(new Product(ProductId.From(1), "Mouse", 12.5m), CancellationToken.None);
    await productRepo.SaveChangesAsync(CancellationToken.None);
    var handler = new AddToCartHandler(cartRepo, productRepo);

    var result = await handler.Handle(new AddToCartCommand(null, ProductId: 1, Quantity: 2), CancellationToken.None);

    await Assert.That(result.IsSuccess).IsTrue();
    await Assert.That(result.Value.Items.Count != 1).IsFalse();
    await Assert.That(result.Value.Total != 25m).IsFalse();
  }

  [Test]
  public async Task Handle_WhenCartMissing_ReturnsNotFound()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var repo = new EfRepository<Cart>(db);
    var handler = new GetCartHandler(repo);

    var result = await handler.Handle(new GetCartQuery(CartId.From(Guid.NewGuid())), CancellationToken.None);

    await Assert.That(result.Status != ResultStatus.NotFound).IsFalse();
  }

  [Test]
  public async Task Handle_WhenCartEmpty_ReturnsInvalid()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var cartRepo = new EfRepository<Cart>(db);
    var guestRepo = new EfRepository<GuestUser>(db);
    var orderRepo = new EfRepository<DomainOrder>(db);
    var cart = await cartRepo.AddAsync(new Cart(), CancellationToken.None);
    await cartRepo.SaveChangesAsync(CancellationToken.None);
    var handler = new CheckoutHandler(cartRepo, guestRepo, orderRepo);

    var result = await handler.Handle(new CheckoutCommand(cart.Id, "guest@local.dev"), CancellationToken.None);

    await Assert.That(result.Status != ResultStatus.Invalid).IsFalse();
  }

  [Test]
  public async Task Handle_WhenCheckoutSucceeds_CreatesOrderGuestUserAndMarksCartDeleted()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var cartRepo = new EfRepository<Cart>(db);
    var guestRepo = new EfRepository<GuestUser>(db);
    var orderRepo = new EfRepository<DomainOrder>(db);
    var cart = new Cart();
    cart.AddItem(11, 3, 2.5m);
    cart = await cartRepo.AddAsync(cart, CancellationToken.None);
    await cartRepo.SaveChangesAsync(CancellationToken.None);
    var handler = new CheckoutHandler(cartRepo, guestRepo, orderRepo);

    var result = await handler.Handle(new CheckoutCommand(cart.Id, "checkout@local.dev"), CancellationToken.None);

    await Assert.That(result.IsSuccess).IsTrue();
    await Assert.That(cart.Deleted).IsTrue();
    await Assert.That(await db.Orders.AnyAsync()).IsTrue();
    await Assert.That(await db.GuestUsers.AnyAsync()).IsTrue();
  }
}

