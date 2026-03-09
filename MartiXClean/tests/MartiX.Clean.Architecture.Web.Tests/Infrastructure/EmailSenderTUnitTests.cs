using MartiX.Clean.Architecture.Web.Infrastructure.Email;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MartiX.Clean.Architecture.Web.Tests.Infrastructure;

public class EmailSenderTUnitTests
{
  [Test]
  public async Task SendEmailAsync_WhenCalled_Completes()
  {
    var logger = Substitute.For<ILogger<FakeEmailSender>>();
    var sender = new FakeEmailSender(logger);

    await sender.SendEmailAsync("to@test.dev", "from@test.dev", "subject", "body");
  }
}

