using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalClean.Architecture.Web.Domain.OrderAggregate;

namespace MinimalClean.Architecture.Web.Infrastructure.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.Property(entity => entity.Id)
      .HasValueGenerator<VogenGuidIdValueGenerator<AppDbContext, Order, OrderId>>()
      .HasVogenConversion()
      .IsRequired();

    builder.Property(entity => entity.CreatedOn)
      .IsRequired();

    builder.Property(entity => entity.GuestUserId)
      .IsRequired();

    builder.Property(entity => entity.DatePaid)
      .IsRequired(false);

    builder.Property(entity => entity.PaymentReference)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired(false);

    // OrderItems relationship
    builder.HasMany(o => o.Items);
  }
}
