using System.Net.Mail;
using Clean.Architecture.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Clean.Architecture.Infrastructure.Email;

/// <summary>
/// MimeKit is recommended over this now:
/// https://weblogs.asp.net/sreejukg/system-net-mail-smtpclient-is-not-recommended-anymore-what-is-the-alternative
/// </summary>
public class SmtpEmailSender(ILogger<SmtpEmailSender> logger,
                       IOptions<MailserverConfiguration> mailserverOptions) : IEmailSender
{
  private readonly ILogger<SmtpEmailSender> _logger = logger;
  private readonly MailserverConfiguration _mailserverConfiguration = mailserverOptions.Value!;

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
    _logger.LogWarning("Sending email to {to} from {from} with subject {subject} using {type}.", to, from, subject, this.ToString());
  }
}
