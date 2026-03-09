using Microsoft.Extensions.Logging;
using Nimble.Modulith.Web;
using NSubstitute;

namespace MartiX.Clean.Architecture.Web.Tests.Shared;

public class LoggingBehaviorTUnitTests
{
  [Test]
  public async Task Handle_WhenInvoked_ExecutesNextAndReturnsResponse()
  {
    var logger = Substitute.For<ILogger<LoggingBehavior<TestMessage, string>>>();
    logger.IsEnabled(LogLevel.Information).Returns(true);
    var behavior = new LoggingBehavior<TestMessage, string>(logger);

    var response = await behavior.Handle(new TestMessage("hello"), static (_, _) => ValueTask.FromResult("ok"), CancellationToken.None);

    await Assert.That(response != "ok").IsFalse();
  }

  public sealed record TestMessage(string Value) : IMessage;
}

