namespace MinimalClean.Architecture.Web.Domain.ProductAggregate;

public class Product : EntityBase<Product, ProductId>, IAggregateRoot
{
  public Product(ProductId id, string name, decimal unitPrice)
  {
    Id = id;
    Name = name;
    UnitPrice = unitPrice;
  }

  public string Name { get; private set; }
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
