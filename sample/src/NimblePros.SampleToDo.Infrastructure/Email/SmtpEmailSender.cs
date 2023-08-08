using System.Net.Mail;
using NimblePros.SampleToDo.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace NimblePros.SampleToDo.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
  private readonly ILogger<SmtpEmailSender> _logger;

  public SmtpEmailSender(ILogger<SmtpEmailSender> logger)
  {
    _logger = logger;
  }

  public async Task SendEmailAsync(string to, string from, string subject, string body)
  {
    var emailClient = new SmtpClient("localhost");
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
