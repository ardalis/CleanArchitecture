namespace MartiX.Clean.Architecture.Web.Domain.ProductAggregate;

public class Product : EntityBase<Product, ProductId>, IAggregateRoot
{
  // Private constructor for EF Core
  private Product() { }

  public Product(ProductId id, string name, decimal unitPrice)
  {
    Id = id;
    Name = name;
    UnitPrice = unitPrice;
  }

  // Factory method for creating new products (before persistence)
  public static Product Create(string name, decimal unitPrice) => new Product(ProductId.New, name, unitPrice);

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
