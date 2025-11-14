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
}
