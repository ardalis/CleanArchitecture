using FastEndpoints;
using MartiX.Clean.Architecture.Web.Feature.Cart.AddToCart;
using MartiX.Clean.Architecture.Web.Feature.Cart.Checkout;
using MartiX.Clean.Architecture.Web.Feature.Product.GetById;
using MartiX.Clean.Architecture.Web.Feature.Product.List;
using CartGetByIdRequest = MartiX.Clean.Architecture.Web.Feature.Cart.GetById.GetCartRequest;

namespace MartiX.Clean.Architecture.Web.Tests.Feature.EndpointPaths;

public class FastEndpointsRequestPathsTUnitTests
{
  [Test]
  public async Task RequestRoutesAndBindings_WhenQueried_AreStable()
  {
    var addToCartRoute = AddToCartRequest.Route;
    var checkoutRoute = CheckoutRequest.Route;
    var getCartRoute = CartGetByIdRequest.Route;
    var getProductRoute = GetProductByIdRequest.Route;
    await Assert.That(addToCartRoute).IsEqualTo("/cart");
    await Assert.That(checkoutRoute).IsEqualTo("/cart/{CartId}/checkout");
    await Assert.That(getCartRoute).IsEqualTo("/cart/{CartId}");
    await Assert.That(getProductRoute).IsEqualTo("/Products/{ProductId}");
    var pageBind = typeof(ListProductsRequest).GetProperty(nameof(ListProductsRequest.Page))
      ?.GetCustomAttributes(typeof(BindFromAttribute), inherit: false)
      .Cast<BindFromAttribute>()
      .SingleOrDefault();
    var perPageBind = typeof(ListProductsRequest).GetProperty(nameof(ListProductsRequest.PerPage))
      ?.GetCustomAttributes(typeof(BindFromAttribute), inherit: false)
      .Cast<BindFromAttribute>()
      .SingleOrDefault();

    await Assert.That(pageBind is null).IsFalse();
    await Assert.That(perPageBind is null).IsFalse();
  }
}

