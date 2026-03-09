using Microsoft.AspNetCore.Http.HttpResults;
using Mediator;
using NSubstitute;
using MartiX.Clean.Architecture.Web.Domain.CartAggregate;
using MartiX.Clean.Architecture.Web.Domain.OrderAggregate;
using MartiX.Clean.Architecture.Web.Feature.Cart;
using MartiX.Clean.Architecture.Web.Feature.Cart.AddToCart;
using MartiX.Clean.Architecture.Web.Feature.Cart.Checkout;
using MartiX.Clean.Architecture.Web.Feature.Cart.GetById;
using MartiX.WebApi.Results;
using MartiX.WebApi.SharedKernel;

namespace MartiX.Clean.Architecture.Web.Tests;

public class CartEndpointAndValidationTests
{
  [Test]
  public async Task AddToCartValidator_WhenInputInvalid_ReturnsInvalid()
  {
    var addValidator = new AddToCartValidator();
    var result = addValidator.Validate(new AddToCartRequest { ProductId = 0, Quantity = 0 });

    await Assert.That(result.IsValid).IsFalse();
  }

  [Test]
  public async Task CheckoutValidator_WhenInputInvalid_ReturnsInvalid()
  {
    var checkoutValidator = new CheckoutValidator();
    var result = checkoutValidator.Validate(new CheckoutRequest { CartId = Guid.Empty, Email = "x" });

    await Assert.That(result.IsValid).IsFalse();
  }

  [Test]
  public async Task AddToCartMapper_WhenEntityProvided_MapsTotalPrice()
  {
    var mapper = new AddToCartMapper();
    var mapped = mapper.FromEntity(CreateCartDto());

    await Assert.That(mapped.Items[0].TotalPrice != 6m).IsFalse();
  }

  [Test]
  public async Task GetCartMapper_WhenEntityProvided_MapsProductId()
  {
    var mapper = new GetCartMapper();
    var mapped = mapper.FromEntity(CreateCartDto());

    await Assert.That(mapped.Items[0].ProductId != 1).IsFalse();
  }

  [Test]
  public async Task CheckoutMapper_WhenEntityProvided_MapsOrderId()
  {
    var mapper = new CheckoutMapper();
    var mapped = mapper.FromEntity(new CheckoutResult(OrderId.From(Guid.NewGuid())));

    await Assert.That(mapped.OrderId == Guid.Empty).IsFalse();
  }

  [Test]
  public async Task AddToCartEndpoint_WhenMediatorReturnsNotFound_MapsNotFound()
  {
    var mediator = Substitute.For<IMediator>();
    mediator.Send(Arg.Any<AddToCartCommand>(), Arg.Any<CancellationToken>())
      .Returns(Result.NotFound("missing"));
    var endpoint = new AddToCartEndpoint(mediator);
    var response = await endpoint.ExecuteAsync(
      new AddToCartRequest { ProductId = 999, Quantity = 1 },
      CancellationToken.None);

    await Assert.That(response.Result is not NotFound).IsFalse();
  }

  [Test]
  public async Task CheckoutEndpoint_WhenMediatorReturnsInvalid_MapsValidationProblem()
  {
    var mediator = Substitute.For<IMediator>();
    mediator.Send(Arg.Any<CheckoutCommand>(), Arg.Any<CancellationToken>())
      .Returns(Result.Invalid(new ValidationError("Cart is empty")));
    var endpoint = new CheckoutEndpoint(mediator);
    var response = await endpoint.ExecuteAsync(
      new CheckoutRequest { CartId = Guid.NewGuid(), Email = "guest@test.dev" },
      CancellationToken.None);

    await Assert.That(response.Result is not ValidationProblem).IsFalse();
  }

  private static CartDto CreateCartDto() =>
    new(
      CartId.From(Guid.NewGuid()),
      new List<CartItemDto> { new(1, 2, 3m, 6m) },
      6m);
}

