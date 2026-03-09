using MartiX.Clean.Architecture.Web.Feature.Cart.AddToCart;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace MartiX.Clean.Architecture.Web.Tests.Feature.Cart;

public class AddToCartEndpointTUnitTests
{
  [Test]
  public async Task ExecuteAsync_WhenResultNotFound_MapsRequestAndReturnsNotFound()
  {
    var mediator = Substitute.For<IMediator>();
    AddToCartCommand? captured = null;
    var requestCartId = Guid.NewGuid();
    mediator.Send(Arg.Do<AddToCartCommand>(command => captured = command), Arg.Any<CancellationToken>())
      .Returns(MartiX.WebApi.Results.Result.NotFound("missing"));

    var endpoint = new AddToCartEndpoint(mediator);
    var response = await endpoint.ExecuteAsync(new AddToCartRequest
    {
      CartId = requestCartId,
      ProductId = 12,
      Quantity = 2
    }, CancellationToken.None);

    await Assert.That(captured).IsNotNull();
    var command = captured!;
    await Assert.That(command.CartId.HasValue && command.CartId.Value.Value == requestCartId).IsTrue();
    await Assert.That(command.ProductId == 12 && command.Quantity == 2).IsTrue();
    await Assert.That(response.Result is not NotFound).IsFalse();
  }

  [Test]
  public async Task ExecuteAsync_WhenResultInvalid_ReturnsProblem()
  {
    var mediator = Substitute.For<IMediator>();
    mediator.Send(Arg.Any<AddToCartCommand>(), Arg.Any<CancellationToken>())
      .Returns(MartiX.WebApi.Results.Result.Invalid(new MartiX.WebApi.Results.ValidationError("Cannot add to cart")));
    var endpoint = new AddToCartEndpoint(mediator);

    var response = await endpoint.ExecuteAsync(new AddToCartRequest { ProductId = 1, Quantity = 1 }, CancellationToken.None);

    await Assert.That(response.Result is not ProblemHttpResult).IsFalse();
  }
}

