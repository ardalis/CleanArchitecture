using NSubstitute;
using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;
using MartiX.Clean.Architecture.Web.Feature.Product;
using MartiX.Clean.Architecture.Web.Feature.Product.GetById;
using MartiX.Clean.Architecture.Web.Feature.Product.List;
using MartiX.Clean.Architecture.Web.Infrastructure.Data;
using MartiX.Clean.Architecture.Web.Infrastructure.Data.Queries;
using MartiX.WebApi.Results;
using MartiX.WebApi.SharedKernel;

namespace MartiX.Clean.Architecture.Web.Tests;

public class ProductHandlersTests
{
  [Test]
  public async Task Handle_WhenProductMissing_ReturnsNotFound()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var repo = new EfRepository<Product>(db);
    var handler = new GetProductHandler(repo);
    var result = await handler.Handle(new GetProductQuery(ProductId.From(1)), CancellationToken.None);

    await Assert.That(result.Status != ResultStatus.NotFound).IsFalse();
  }

  [Test]
  public async Task Handle_WhenProductExists_ReturnsSuccess()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var repo = new EfRepository<Product>(db);
    await repo.AddAsync(new Product(ProductId.From(2), "Keyboard", 22m), CancellationToken.None);
    await repo.SaveChangesAsync(CancellationToken.None);
    var handler = new GetProductHandler(repo);

    var result = await handler.Handle(new GetProductQuery(ProductId.From(2)), CancellationToken.None);

    await Assert.That(result.IsSuccess && result.Value.Name == "Keyboard").IsTrue();
  }

  [Test]
  public async Task Handle_WhenQueryReturnsPage_ReturnsSuccessResult()
  {
    var query = Substitute.For<IListProductsQueryService>();
    query.ListAsync(2, 5).Returns(new PagedResult<ProductDto>(new List<ProductDto>(), 2, 5, 0, 0));
    var handler = new ListProductsHandler(query);

    var result = await handler.Handle(new ListProductsQuery(Page: 2, PerPage: 5), CancellationToken.None);

    await Assert.That(result.IsSuccess).IsTrue();
    await Assert.That(result.Value.Page != 2 || result.Value.PerPage != 5).IsFalse();
  }

  [Test]
  public async Task ListAsync_WhenPagingRequested_ReturnsExpectedPageAndCounts()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    db.Products.AddRange(
      new Product(ProductId.From(1), "A", 1m),
      new Product(ProductId.From(2), "B", 2m),
      new Product(ProductId.From(3), "C", 3m));
    await db.SaveChangesAsync();
    var service = new ListProductsQueryService(db);

    var result = await service.ListAsync(page: 2, perPage: 2);

    await Assert.That(result.Items.Count != 1).IsFalse();
    await Assert.That(result.TotalCount != 3 || result.TotalPages != 2).IsFalse();
  }
}

