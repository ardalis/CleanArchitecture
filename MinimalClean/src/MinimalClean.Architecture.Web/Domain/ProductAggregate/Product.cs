using Ardalis.GuardClauses;

namespace MinimalClean.Architecture.Web.Domain.ProductAggregate;

public class Product : EntityBase<Product, ProductId>, IAggregateRoot
{
  // Private constructor for EF Core
  private Product() { }

  // Private constructor for new (unpersisted) products
  private Product(string name, decimal unitPrice)
  {
    Name = name;
    UnitPrice = unitPrice;
  }

  // Constructor for reconstituting persisted products with a known ID
  public Product(ProductId id, string name, decimal unitPrice)
  {
    Guard.Against.InvalidInput(id, nameof(id), (id) => id != ProductId.New,
      "Use Product.Create() to create new products instead of passing ProductId.New to the constructor.");
    Id = id;
    Name = name;
    UnitPrice = unitPrice;
  }

  // Factory method for creating new products (before persistence)
  public static Product Create(string name, decimal unitPrice) => new Product(name, unitPrice);

  public string Name { get; private set; } = string.Empty;
  public decimal UnitPrice { get; private set; }

  public Product UpdateName(string newName)
  {
    Name = newName;
    return this;
  }

  public Product UpdatePrice(decimal newPrice)
  {
    UnitPrice = newPrice;
    return this;
  }
}
