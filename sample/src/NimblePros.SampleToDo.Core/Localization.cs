using Microsoft.Extensions.Localization;

/// <summary>
/// Exposes the current <see cref="IStringLocalizer"/> to static code
/// (Vogen Validate, domain events, etc.) without pulling the whole
/// DI container into Core.
/// </summary>
public static class Localization
{
  /// <summary>
  /// Set by <c>Program.cs</c> during app startup.
  /// </summary>
  public static ILocalizationContext? Current { get; set; }

  public sealed class LocalizationContext : ILocalizationContext
  {
    public IStringLocalizer Localizer { get; }

    public LocalizationContext(IStringLocalizer localizer) =>
        Localizer = localizer;
  }
}

public interface ILocalizationContext
{
  IStringLocalizer Localizer { get; }
}
