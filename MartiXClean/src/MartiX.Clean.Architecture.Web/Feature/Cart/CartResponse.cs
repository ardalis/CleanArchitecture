namespace MartiX.Clean.Architecture.Web.Feature.Cart;

public record CartResponse(Guid CartId, IReadOnlyList<CartItemResponse> Items, decimal Total);

public record CartItemResponse(int ProductId, int Quantity, decimal UnitPrice, decimal TotalPrice);

