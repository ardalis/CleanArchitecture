namespace MinimalClean.Architecture.Web.Domain.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string to, string from, string subject, string body);
}
