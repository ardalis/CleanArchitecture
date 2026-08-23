using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Infrastructure.Email;

public partial class MimeKitEmailSender(
  ILogger<MimeKitEmailSender> logger,
  IOptions<MailserverConfiguration> mailserverOptions)
  : IEmailSender
{
  private readonly ILogger<MimeKitEmailSender> _logger = logger;

  private readonly MailserverConfiguration _mailserverConfiguration =
    mailserverOptions.Value!;

  public async Task SendEmailAsync(
    string recipient,
    string from,
    string subject,
    string body)
  {
    LogSendingEmail(
      _logger,
      recipient,
      from,
      subject,
      nameof(MimeKitEmailSender));

    using var client = new MailKit.Net.Smtp.SmtpClient();

    await client
      .ConnectAsync(
        _mailserverConfiguration.Hostname,
        _mailserverConfiguration.Port,
        false)
      .ConfigureAwait(false);

    using var message = new MimeMessage();

    message.From.Add(new MailboxAddress(from, from));
    message.To.Add(new MailboxAddress(recipient, recipient));
    message.Subject = subject;
    message.Body = new TextPart("plain") { Text = body };

    await client
      .SendAsync(message)
      .ConfigureAwait(false);

    await client
      .DisconnectAsync(
        true,
        new CancellationToken(canceled: true))
      .ConfigureAwait(false);
  }

  [LoggerMessage(
    EventId = 2,
    Level = LogLevel.Warning,
    Message = "Sending email to {Recipient} from {From} with subject {Subject} using {SenderType}.")]
  private static partial void LogSendingEmail(
    ILogger logger,
    string recipient,
    string from,
    string subject,
    string senderType);
}
