using NimblePros.SampleToDo.Core.ContributorAggregate;
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

    builder.Property(p => p.Title)
      .HasVogenConversion()
      .HasMaxLength(ToDoItemTitle.MaxLength)
      .IsRequired();

    builder.Property(p => p.Description)
      .HasVogenConversion()
      .HasMaxLength(ToDoItemDescription.MaxLength)
      .IsRequired();

    builder.Property(t => t.ContributorId)
      .HasConversion(
          v => v.HasValue ? v.Value.Value : (int?)null, // to db
          v => v.HasValue ? ContributorId.From(v.Value) : (ContributorId?)null // from db
      )
      .IsRequired(false);
    builder.Property(t => t.Priority)
      .HasConversion(
          p => p.Value,
          p => Priority.FromValue(p));
  }
}
