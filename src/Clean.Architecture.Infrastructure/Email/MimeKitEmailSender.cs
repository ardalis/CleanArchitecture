using Clean.Architecture.Core.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Clean.Architecture.Infrastructure.Email;

public class MimeKitEmailSender(ILogger<MimeKitEmailSender> _logger) : IEmailSender
{
  public async Task SendEmailAsync(string to, string from, string subject, string body)
  {
    _logger.LogInformation("Attempting to send email to {to} from {from} with subject {subject}...", to, from, subject);

    using (SmtpClient client = new SmtpClient()) // use localhost and a test server
    {
      client.Connect("localhost", 25, false); // TODO: pull settings from config
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(from, from));
      message.To.Add(new MailboxAddress(to, to));
      message.Subject = subject;
      message.Body = new TextPart("plain") { Text = body };

      await client.SendAsync(message);
      _logger.LogInformation("Email sent!");

      client.Disconnect(true);
    }
  }
}
