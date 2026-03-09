using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;
using MartiX.Clean.Architecture.Web.Domain.ProductAggregate.Specifications;
using ProductEntity = MartiX.Clean.Architecture.Web.Domain.ProductAggregate.Product;

namespace MartiX.Clean.Architecture.Web.Feature.Product.GetById;

public record GetProductQuery(ProductId ProductId) : IQuery<Result<ProductDto>>;

public class GetProductHandler(IReadRepository<ProductEntity> _repository)
  : IQueryHandler<GetProductQuery, Result<ProductDto>>
{
  public async ValueTask<Result<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
  {
    var spec = new ProductByIdSpec(request.ProductId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new ProductDto(entity.Id, entity.Name, entity.UnitPrice);
  }
}

