namespace MinimalClean.Architecture.Web.ProductFeatures.List;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListProductsQueryService
{
  Task<PagedResult<ProductDto>> ListAsync(int page, int perPage);
}
