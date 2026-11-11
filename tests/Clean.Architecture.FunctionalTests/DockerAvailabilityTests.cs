using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;

namespace Clean.Architecture.FunctionalTests;

public class DockerAvailabilityTests
{
  [Fact]
  public async Task Docker_ShouldBeRunning_ForFullFunctionalTestCoverage()
  {
    var cancellationToken = TestContext.Current.CancellationToken;

    try
    {
      await using var container = new MsSqlBuilder(
          "mcr.microsoft.com/mssql/server:2025-latest")
        .WithPassword("Your_password123!")
        .Build();

      await container.StartAsync(cancellationToken);
    }
    catch (DockerUnavailableException ex)
    {
      Assert.Skip(
        "Docker is not running or is unavailable. " +
        "Functional tests can still run with SQLite, " +
        "but SQL Server-specific behavior is not covered. " +
        $"Error: {ex.Message}");
    }
  }
}
