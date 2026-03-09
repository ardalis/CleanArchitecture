using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;

namespace MartiX.Clean.Architecture.Web.Feature.Product;

public record ProductDto(ProductId Id, string Name, decimal UnitPrice);

