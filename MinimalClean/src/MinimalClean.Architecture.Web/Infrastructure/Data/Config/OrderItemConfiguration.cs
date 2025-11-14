using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalClean.Architecture.Web.Domain.OrderAggregate;
using MinimalClean.Architecture.Web.Domain.ProductAggregate;

namespace MinimalClean.Architecture.Web.Infrastructure.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
  public void Configure(EntityTypeBuilder<OrderItem> builder)
  {
    ArgumentNullException.ThrowIfNull(builder);
    builder.ToTable("OrderItems");

    builder.Property(entity => entity.Id)
      .HasValueGenerator<VogenGuidIdValueGenerator<AppDbContext, OrderItem, OrderItemId>>()
      .HasVogenConversion()
      .IsRequired();
        
    builder.Property(oi => oi.OrderId)
      .HasVogenConversion()
      .IsRequired();
    
    builder.Property(oi => oi.ProductId)
      .HasVogenConversion()
      .IsRequired();
    
    builder.Property(oi => oi.Quantity)
      .HasVogenConversion()
      .IsRequired();
    
    builder.Property(oi => oi.UnitPrice)
      .HasVogenConversion()
      .HasPrecision(18, 2)
      .IsRequired();
  }
}
