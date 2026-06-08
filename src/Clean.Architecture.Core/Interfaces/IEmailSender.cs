namespace Clean.Architecture.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string recipientEmail, string senderEmail, string subject, string body);
}
