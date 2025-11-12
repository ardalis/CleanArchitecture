// Core/ProjectAggregate/ProjectMessages.cs
namespace NimblePros.SampleToDo.Core.ProjectAggregate;

/// <summary>
/// Strongly-typed, localisable message accessors for the Project aggregate.
/// The implementation is deliberately **static** so it can be called from Vogen’s
/// static <c>Validate</c> method.
/// </summary>
public static class ProjectErrorMessages
{
  // -----------------------------------------------------------------
  // 1. Public entry points – these are what you call from the domain
  // -----------------------------------------------------------------
  public static string CoreProjectNameEmpty => Get(nameof(CoreProjectNameEmpty));
  public static string CoreProjectNameTooLong(int maxLength) => Get(nameof(CoreProjectNameTooLong), maxLength);
  public static string CoreToDoItemDescriptionEmpty => Get(nameof(CoreToDoItemDescriptionEmpty));
  public static string CoreToDoItemDescriptionTooLong(int maxLength) => Get(nameof(CoreToDoItemDescriptionTooLong), maxLength);
  public static string CoreToDoItemTitleEmpty => Get(nameof(CoreToDoItemTitleEmpty));
  public static string CoreToDoItemTitleTooLong(int maxLength) => Get(nameof(CoreToDoItemTitleTooLong), maxLength);

  // -----------------------------------------------------------------
  // 2. Private helper that forwards to the current localizer
  // -----------------------------------------------------------------
  private static string Get(string key, params object[] args)
  {
    // The static holder is set once in Program.cs (Web project)
    var localizer = Localization.Current?.Localizer;

    if (localizer is not null)
    {
      // Uses the standard {0}, {1}… placeholders defined in JSON/RESX
      var localized = localizer[key, args];
      return localized.ResourceNotFound ? Fallback(key, args) : localized;
    }

    // No DI container available (e.g. unit-tests) → fallback to English
    return Fallback(key, args);
  }

  // -----------------------------------------------------------------
  // 3. Hard-coded English fallback (never throws, always returns a string)
  // -----------------------------------------------------------------
  private static string Fallback(string key, object[] args) => key switch
  {
    nameof(CoreProjectNameEmpty) => "Name cannot be empty",
    nameof(CoreProjectNameTooLong) => FormattableString.Invariant($"Name cannot be longer than {args[0]} characters"),
    nameof(CoreToDoItemDescriptionEmpty) => "Description cannot be empty",
    nameof(CoreToDoItemDescriptionTooLong) => FormattableString.Invariant($"Description cannot be longer than {args[0]} characters"),
    nameof(CoreToDoItemTitleEmpty) => "Title cannot be empty",
    nameof(CoreToDoItemTitleTooLong) => FormattableString.Invariant($"Title cannot be longer than {args[0]} characters"),
    _ => $"[{key}]"
  };
}
