using MinimalClean.Architecture.Web.Domain.ProductAggregate;

namespace MinimalClean.Architecture.Web.ProductFeatures;
public record ProductDto(ProductId Id, string Name, decimal UnitPrice);
