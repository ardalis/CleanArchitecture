namespace MinimalClean.Architecture.Web.Domain.ProductAggregate.Specifications;

public class ProductByIdSpec : Specification<Product>
{
  public ProductByIdSpec(ProductId productId) =>
    Query
        .Where(product => product.Id == productId);
}
