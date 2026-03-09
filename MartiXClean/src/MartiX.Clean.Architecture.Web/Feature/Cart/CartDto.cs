using MartiX.Clean.Architecture.Web.Domain.CartAggregate;

namespace MartiX.Clean.Architecture.Web.Feature.Cart;

public record CartDto(CartId Id, IReadOnlyList<CartItemDto> Items, decimal Total);

public record CartItemDto(int ProductId, int Quantity, decimal UnitPrice, decimal TotalPrice);

