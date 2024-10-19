using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
  public void Configure(EntityTypeBuilder<Project> builder)
  {
    builder.Property(p => p.Id)
      .HasValueGenerator<VogenIdValueGenerator<AppDbContext, Project, ProjectId>>()
      .HasVogenConversion()
      .IsRequired();
    builder.Property(p => p.Name)
      .HasVogenConversion()
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();
  }
}


