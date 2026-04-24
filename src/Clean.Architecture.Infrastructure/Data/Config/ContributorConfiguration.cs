using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.Infrastructure.Data.Config;

public class ContributorConfiguration : IEntityTypeConfiguration<Contributor>
{
  public void Configure(EntityTypeBuilder<Contributor> builder)
  {
    builder.Property(entity => entity.Id)
      .ValueGeneratedOnAdd()
      .HasVogenConversion()
      .IsRequired();

    builder.Property(entity => entity.Name)
      .HasVogenConversion()
      .HasMaxLength(ContributorName.MaxLength)
      .IsRequired();

    builder.OwnsOne(builder => builder.PhoneNumber);

    builder.Property(x => x.Status)
      .HasConversion(
          x => x.Value,
          x => ContributorStatus.FromValue(x));
  }
}
