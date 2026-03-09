using MartiX.Clean.Architecture.Web.Feature.Product.Create;
using MartiX.Clean.Architecture.Web.Feature.Product;
using MartiX.WebApi.SharedKernel;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using ProductEntity = MartiX.Clean.Architecture.Web.Domain.ProductAggregate.Product;

namespace MartiX.Clean.Architecture.Web.Tests.Feature.Product;

public class CreateEndpointTUnitTests
{
  [Test]
  public async Task ExecuteAsync_WhenCreateSucceeds_ReturnsCreatedProductRecord()
  {
    var repository = Substitute.For<IRepository<ProductEntity>>();
    repository.AddAsync(Arg.Any<ProductEntity>(), Arg.Any<CancellationToken>())
      .Returns(call => call.Arg<ProductEntity>());
    repository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
    var endpoint = new CreateEndpoint(repository);

    var response = await endpoint.ExecuteAsync(new CreateProductRequest { Name = "Created via endpoint", UnitPrice = 9.9m }, CancellationToken.None);

    await Assert.That(response.Result is not Created<ProductRecord>).IsFalse();
  }
}

