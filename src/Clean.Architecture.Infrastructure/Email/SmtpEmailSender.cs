using System.Net.Mail;
using Clean.Architecture.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Clean.Architecture.Infrastructure.Email;

public class SmtpEmailSender(ILogger<SmtpEmailSender> _logger) : IEmailSender
{
  public async Task SendEmailAsync(string to, string from, string subject, string body)
  {
    var emailClient = new SmtpClient("localhost"); // TODO: pull settings from config
    var message = new MailMessage
    {
      From = new MailAddress(from),
      Subject = subject,
      Body = body
    };
    message.To.Add(new MailAddress(to));
    await emailClient.SendMailAsync(message);
    _logger.LogWarning("Sending email to {to} from {from} with subject {subject}.", to, from, subject);
  }
}
