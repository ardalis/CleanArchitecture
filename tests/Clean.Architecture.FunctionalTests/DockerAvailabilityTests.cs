using Docker.DotNet;

namespace Clean.Architecture.FunctionalTests;

public class DockerAvailabilityTests
{
  [Fact]
  public async Task Docker_ShouldBeRunning_ForFullFunctionalTestCoverage()
  {
    var cancellationToken = TestContext.Current.CancellationToken;

    // Capture any Docker client failure without a broad catch block (CA1031).
    // Any failure to ping means Docker is unavailable or misconfigured for this test.
    using var client = new DockerClientBuilder().Build();
    var exception = await Record.ExceptionAsync(
      () => client.System.PingAsync(cancellationToken))
      .ConfigureAwait(true);

    if (exception is not null)
    {
      Assert.Fail(
        "Docker is not running or is misconfigured. " +
        "Functional tests that use SQL Server will fall back to SQLite, " +
        "which may not catch SQL Server-specific issues. " +
        "For full test coverage, please start Docker Desktop and re-run the tests. " +
        $"Underlying error: {exception.Message}");
    }
  }
}
