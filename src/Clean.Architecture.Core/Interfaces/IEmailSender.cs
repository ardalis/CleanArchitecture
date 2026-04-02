namespace Clean.Architecture.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string toAddress, string from, string subject, string body);
}
