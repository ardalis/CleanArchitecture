using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Infrastructure.Email;

public partial class FakeEmailSender(
  ILogger<FakeEmailSender> logger)
  : IEmailSender
{
  private readonly ILogger<FakeEmailSender> _logger = logger;

  public Task SendEmailAsync(
    string recipient,
    string from,
    string subject,
    string body)
  {
    LogEmailNotSent(_logger, recipient, from, subject);

    return Task.CompletedTask;
  }

  [LoggerMessage(
    EventId = 1,
    Level = LogLevel.Information,
    Message = "Not actually sending an email to {Recipient} from {From} with subject {Subject}")]
  private static partial void LogEmailNotSent(
    ILogger logger,
    string recipient,
    string from,
    string subject);
}
