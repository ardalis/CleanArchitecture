namespace Clean.Architecture.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string recipient, string from, string subject, string body);
}
