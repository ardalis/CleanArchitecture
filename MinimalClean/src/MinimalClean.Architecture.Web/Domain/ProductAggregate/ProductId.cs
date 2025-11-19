using Vogen;

namespace MinimalClean.Architecture.Web.Domain.ProductAggregate;

[ValueObject<int>]
public readonly partial struct ProductId
{
  /// <summary>
  /// Represents a new Product that has not yet been persisted to the database.
  /// EF Core will assign the actual identity value on SaveChanges.
  /// </summary>
  public static ProductId New => From(0);

  private static Validation Validate(int value)
      => value >= 0 ? Validation.Ok : Validation.Invalid("ProductId must be non-negative.");
}
