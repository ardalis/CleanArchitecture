using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalClean.Architecture.Web.Domain.ProductAggregate;

namespace MinimalClean.Architecture.Web.Infrastructure.Data.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.Property(entity => entity.Id)
      .HasValueGenerator<VogenIntIdValueGenerator<AppDbContext, Product, ProductId>>()
      .HasVogenConversion()
      .IsRequired();

    builder.Property(entity => entity.Name)
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(entity => entity.UnitPrice)
      .HasPrecision(18, 2)
      .IsRequired();

    builder.HasData(
      new Product(
        ProductId.From(1),
        "Coffee Mug",
        9.99m),
      new Product(
        ProductId.From(2),
        "T-Shirt",
        19.99m),
      new Product(
        ProductId.From(3),
        "Sticker Pack",
        3.99m)
    );
  }
}
