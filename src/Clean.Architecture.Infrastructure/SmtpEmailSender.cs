using System.Net.Mail;
using Clean.Architecture.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Clean.Architecture.Infrastructure;

public class SmtpEmailSender : IEmailSender
{
  private readonly ILogger<SmtpEmailSender> _logger;
  private readonly MailserverConfiguration _mailserverConfiguration;

  public SmtpEmailSender(ILogger<SmtpEmailSender> logger, IOptions<MailserverConfiguration> mailserverOptions)
  {
    _logger = logger;
    _mailserverConfiguration = mailserverOptions.Value;
  }

  public async Task SendEmailAsync(string to, string from, string subject, string body)
  {
    var emailClient = new SmtpClient(_mailserverConfiguration.Hostname, _mailserverConfiguration.Port);
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
