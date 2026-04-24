using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

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
  }
}
