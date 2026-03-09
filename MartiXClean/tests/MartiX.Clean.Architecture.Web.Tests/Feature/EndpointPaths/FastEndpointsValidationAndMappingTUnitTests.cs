using MartiX.Clean.Architecture.Web.Domain.CartAggregate;
using MartiX.Clean.Architecture.Web.Domain.OrderAggregate;
using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;
using MartiX.Clean.Architecture.Web.Feature.Cart;
using MartiX.Clean.Architecture.Web.Feature.Cart.AddToCart;
using MartiX.Clean.Architecture.Web.Feature.Cart.Checkout;
using MartiX.Clean.Architecture.Web.Feature.Product;
using MartiX.Clean.Architecture.Web.Feature.Product.GetById;
using MartiX.Clean.Architecture.Web.Feature.Product.List;
using MartiX.WebApi.SharedKernel;

namespace MartiX.Clean.Architecture.Web.Tests.Feature.EndpointPaths;

public class FastEndpointsValidationAndMappingTUnitTests
{
  [Test]
  public async Task GetProductByIdValidator_WhenRequestValid_ReturnsValid()
  {
    var getProductValidator = new GetProductByIdValidator();
    var result = getProductValidator.Validate(new GetProductByIdRequest { ProductId = 42 });

    await Assert.That(result.IsValid).IsTrue();
  }

  [Test]
  public async Task AddToCartValidator_WhenRequestValid_ReturnsValid()
  {
    var addToCartValidator = new AddToCartValidator();
    var result = addToCartValidator.Validate(new AddToCartRequest { ProductId = 1, Quantity = 1 });

    await Assert.That(result.IsValid).IsTrue();
  }

  [Test]
  public async Task CheckoutValidator_WhenRequestValid_ReturnsValid()
  {
    var checkoutValidator = new CheckoutValidator();
    var result = checkoutValidator.Validate(new CheckoutRequest { CartId = Guid.NewGuid(), Email = "guest@test.dev" });

    await Assert.That(result.IsValid).IsTrue();
  }

  [Test]
  public async Task ListProductsValidator_WhenRequestValid_ReturnsValid()
  {
    var listValidator = new ListProductsValidator();
    var result = listValidator.Validate(new ListProductsRequest { Page = 1, PerPage = Constants.MAX_PAGE_SIZE });

    await Assert.That(result.IsValid).IsTrue();
  }

  [Test]
  public async Task AddToCartMapper_WhenEntityProvided_MapsItemTotal()
  {
    var addToCartMapper = new AddToCartMapper();
    var mapped = addToCartMapper.FromEntity(new CartDto(
      CartId.From(Guid.NewGuid()),
      new List<CartItemDto> { new(1, 2, 3m, 6m) },
      6m));

    await Assert.That(mapped.Items[0].TotalPrice != 6m).IsFalse();
  }

  [Test]
  public async Task CheckoutMapper_WhenEntityProvided_MapsOrderId()
  {
    var checkoutMapper = new CheckoutMapper();
    var mapped = checkoutMapper.FromEntity(new CheckoutResult(OrderId.From(Guid.NewGuid())));

    await Assert.That(mapped.OrderId == Guid.Empty).IsFalse();
  }

  [Test]
  public async Task ListProductsMapper_WhenEntityProvided_MapsProductRecords()
  {
    var listMapper = new ListProductsMapper();
    var mapped = listMapper.FromEntity(new PagedResult<ProductDto>(
      new List<ProductDto> { new(ProductId.From(3), "Mapped", 7m) },
      1, 10, 1, 1));

    await Assert.That(mapped.Items.Count != 1 || mapped.Items[0].Id != 3).IsFalse();
  }
}

