using Docker.DotNet;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace Clean.Architecture.FunctionalTests;

/// <summary>
/// Checks whether the local Docker engine can run the Linux SQL Server image.
/// </summary>
internal static class DockerEnvironment
{
  /// <summary>
  /// Throws <see cref="DockerUnavailableException"/> when Docker is not reachable or
  /// is running Windows containers (the default on GitHub-hosted Windows runners).
  /// In that mode the Linux SQL Server image can't be pulled and Testcontainers
  /// reports a misleading "No such image" error instead of Docker being unavailable.
  /// </summary>
  public static async Task EnsureLinuxContainersAvailableAsync(
    CancellationToken cancellationToken = default)
  {
    var endpoint = TestcontainersSettings.OS.DockerEndpointAuthConfig
      ?? throw new DockerUnavailableException(
        "No Docker endpoint was found. Docker is not running or is not installed.");

    string? osType;

    try
    {
      using var client = new DockerClientBuilder()
        .WithEndpoint(endpoint.Endpoint)
        .WithTimeout(TimeSpan.FromSeconds(10))
        .Build();

      var info = await client.System.GetSystemInfoAsync(cancellationToken);
      osType = info.OSType;
    }
    catch (Exception ex) when (ex is not OperationCanceledException)
    {
      // The probe is best-effort (e.g. it doesn't apply TLS settings). If it can't
      // answer, let Testcontainers start the container and report the real error.
      return;
    }

    if (!string.Equals(osType, "linux", StringComparison.OrdinalIgnoreCase))
    {
      throw new DockerUnavailableException(
        $"Docker is running '{osType}' containers, but the SQL Server test " +
        "container requires Linux containers.");
    }
  }
}
