using NimblePros.SampleToDo.Core.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
{
  public void Configure(EntityTypeBuilder<ToDoItem> builder)
  {
    // TODO: Use Vogen to generate this
    builder.Property(t => t.Id)
      .HasConversion(
        t => t.Value,
        t => ToDoItemId.From(t))
      .IsRequired();
    builder.Property(t => t.Title)
        .IsRequired();
    builder.Property(t => t.ContributorId)
        .IsRequired(false);
    builder.Property(t => t.Priority)
      .HasConversion(
          p => p.Value,
          p => Priority.FromValue(p));
  }
}
