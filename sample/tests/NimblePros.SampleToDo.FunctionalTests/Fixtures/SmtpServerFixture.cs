using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

namespace NimblePros.SampleToDo.FunctionalTests.ClassFixtures;

/// <summary>
/// This class ensures that an SMTP server is running and shared between all tests in a specified class.
/// </summary>
public class SmtpServerFixture : IAsyncLifetime
{
  private const string SmtpServerImageName = "jijiechen/papercut:latest";
  private const int SmtpServerListenPort = 25;

  private IContainer? _container;

  public async Task InitializeAsync()
  {
    _container = new ContainerBuilder()
        .WithName(Guid.NewGuid().ToString("D"))
        .WithImage(SmtpServerImageName)
        .WithPortBinding(SmtpServerListenPort, SmtpServerListenPort)
        .WithWaitStrategy(
          Wait
          .ForUnixContainer()
          .UntilMessageIsLogged("Server Ready", o => o.WithTimeout(TimeSpan.FromSeconds(30))))
        .Build();

    await _container.StartAsync().ConfigureAwait(false);
  }

  public async Task DisposeAsync()
  {
    if (_container != null)
    {
      await _container.StopAsync().ConfigureAwait(false);
      await _container.DisposeAsync().ConfigureAwait(false);
      _container = null;
    }
  }

  /// <summary>
  /// Ensures that the container is running and healthy.
  /// </summary>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the SMTP server container was not created or is not running.
  /// This could be due to Docker not running or issues with the container image.
  /// In such cases, verify that Docker is running correctly.
  /// </exception>
  public void EnsureContainerIsRunning()
  {
    if (_container == null)
    {
      throw new InvalidOperationException("SMTP server container was not created. Ensure Docker is running and the container image is correct.");
    }

    if (_container.State != TestcontainersStates.Running)
    {
      throw new InvalidOperationException("The SMTP server container is not running. Please verify that the SMTP server image is correctly configured and that Docker is functioning properly.");
    }
  }
}
