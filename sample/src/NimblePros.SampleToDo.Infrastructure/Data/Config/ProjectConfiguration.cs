using NimblePros.SampleToDo.Core.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
  public void Configure(EntityTypeBuilder<Project> builder)
  {
    builder.Property(p => p.Id)
      .HasValueGenerator<VogenIdValueGenerator<AppDbContext, Project, ProjectId>>();
    builder.Property(p => p.Name)
      .HasVogenConversion()
      //.HasConversion(new EfCoreValueConverter(), new EfCoreValueComparer())
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();
  }
}


