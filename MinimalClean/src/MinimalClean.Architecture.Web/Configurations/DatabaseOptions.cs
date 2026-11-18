namespace MinimalClean.Architecture.Web.Configurations;

/// <summary>
/// Configuration options for database management
/// </summary>
public class DatabaseOptions
{
  /// <summary>
  /// If true, drops and recreates the database on startup in Development environment.
  /// WARNING: This will delete all existing data!
  /// </summary>
  public bool RecreateOnStartup { get; set; } = false;

  /// <summary>
  /// If true, applies migrations and seeds data on startup outside the Development environment
  /// (useful for containerized environments). Migrations always run in Development.
  /// A failed migration stops the app from starting.
  /// </summary>
  public bool ApplyMigrationsOnStartup { get; set; } = false;
}
