using NimblePros.SampleToDo.Core.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vogen;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
  public void Configure(EntityTypeBuilder<Project> builder)
  {
    builder.Property(p => p.Name)
      //.HasVogenConversion()
      .HasConversion(new EfCoreValueConverter(), new EfCoreValueComparer())
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder.Property(p => p.Priority)
      .HasConversion(
          p => p.Value,
          p => Priority.FromValue(p));
  }

  /// <summary>
  /// See: https://github.com/SteveDunn/Vogen/issues/676
  /// I'm hoping this can be generated here. I don't want to have to do this for every value object.
  /// And i don't want the EF Core stuff to be in my Core project
  /// </summary>
  public class EfCoreValueConverter : ValueConverter<ProjectName, String>
  {
    public EfCoreValueConverter() : this(null)
    {
    }

    public EfCoreValueConverter(ConverterMappingHints mappingHints = null)
      : base(vo => vo.Value, value => ProjectName.From(value), mappingHints)
    {
    }
  }

  public class EfCoreValueComparer : ValueComparer<ProjectName>
  {
    public EfCoreValueComparer() : base((left, right) => DoCompare(left, right), 
      instance => instance.IsInitialized() ? instance.Value.GetHashCode() : 0)
    {
    }

    static bool DoCompare(ProjectName left, ProjectName right)
    {
      // if both null, then they're equal
      if (left is null)
        return right is null;
      // if only right is null, then they're not equal
      if (right is null)
        return false;
      // if they're both the same reference, then they're equal
      if (ReferenceEquals(left, right))
        return true;
      // if neither are initialized, then they're equal
      if (!left.IsInitialized() && !right.IsInitialized())
        return true;
      return left.IsInitialized() && right.IsInitialized() && left.Value.Equals(right.Value);
    }
  }

}


