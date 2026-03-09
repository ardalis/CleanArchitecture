using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using MartiX.Clean.Architecture.Web.Domain.Interfaces;
using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;
using MartiX.Clean.Architecture.Web.Feature.Product;
using MartiX.Clean.Architecture.Web.Feature.Product.Create;
using MartiX.Clean.Architecture.Web.Feature.Product.GetById;
using MartiX.Clean.Architecture.Web.Feature.Product.List;
using MartiX.WebApi.SharedKernel;

namespace MartiX.Clean.Architecture.Web.Tests;

public class ProductEndpointAndValidationTests
{
  [Test]
  [Arguments("CreateProduct")]
  [Arguments("ListProducts")]
  public async Task Validator_WhenInputInvalid_ReturnsInvalid(string validatorCase)
  {
    var result = validatorCase switch
    {
      "CreateProduct" => new CreateProductValidator().Validate(new CreateProductRequest { Name = string.Empty, UnitPrice = 0 }),
      "ListProducts" => new ListProductsValidator().Validate(new ListProductsRequest { Page = 0, PerPage = Constants.MAX_PAGE_SIZE + 1 }),
      _ => throw new ArgumentOutOfRangeException(nameof(validatorCase), validatorCase, null)
    };

    await Assert.That(result.IsValid).IsFalse();
  }

  [Test]
  public async Task CreateProductValidator_WhenInputValid_ReturnsValid()
  {
    var createValidator = new CreateProductValidator();
    var result = createValidator.Validate(new CreateProductRequest { Name = "P", UnitPrice = 1 });

    await Assert.That(result.IsValid).IsTrue();
  }

  [Test]
  public async Task ListProductsMapper_WhenEntityProvided_MapsItems()
  {
    var mapper = new ListProductsMapper();
    var paged = new PagedResult<ProductDto>(
      new List<ProductDto> { new(ProductId.From(1), "A", 2m) },
      1, 10, 1, 1);
    var mapped = mapper.FromEntity(paged);

    await Assert.That(mapped.Items.Count != 1).IsFalse();
  }

  [Test]
  public async Task GetProductByIdMapper_WhenEntityProvided_MapsId()
  {
    var mapper = new GetProductByIdMapper();
    var mapped = mapper.FromEntity(new ProductDto(ProductId.From(2), "B", 3m));

    await Assert.That(mapped.Id != 2).IsFalse();
  }

  [Test]
  public async Task ExecuteAsync_WhenCreateSucceeds_ReturnsCreated()
  {
    var productRepo = Substitute.For<IRepository<Product>>();
    productRepo.AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
      .Returns(call => call.Arg<Product>());
    productRepo.SaveChangesAsync(Arg.Any<CancellationToken>())
      .Returns(1);
    var createEndpoint = new CreateEndpoint(productRepo);
    var createResponse = await createEndpoint.ExecuteAsync(
      new CreateProductRequest { Name = "Created", UnitPrice = 10m },
      CancellationToken.None);
    await Assert.That(createResponse.Result is not Created<ProductRecord>).IsFalse();
  }
}

