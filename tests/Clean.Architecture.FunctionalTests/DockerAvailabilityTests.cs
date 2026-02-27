using Docker.DotNet;

namespace Clean.Architecture.FunctionalTests;

public class DockerAvailabilityTests
{
  [Fact]
  public async Task Docker_ShouldBeRunning_ForFullFunctionalTestCoverage()
  {
    try
    {
      // Ping the Docker daemon directly using the Docker client.
      // This has no side effects on container lifecycle or Testcontainers internals.
      using var client = new DockerClientConfiguration().CreateClient();
      await client.System.PingAsync();
    }
    catch (Exception)
    {
      Assert.Fail(
        "Docker is not running or is misconfigured. " +
        "Functional tests that use SQL Server will fall back to SQLite, which may not catch SQL Server-specific issues. " +
        "For full test coverage, please start Docker Desktop (https://www.docker.com/products/docker-desktop/) and re-run the tests.");
    }
  }
}
