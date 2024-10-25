using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
{
  public void Configure(EntityTypeBuilder<ToDoItem> builder)
  {
    builder.Property(p => p.Id)
      .HasValueGenerator<VogenIdValueGenerator<AppDbContext, ToDoItem, ToDoItemId>>()
      .HasVogenConversion()
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
