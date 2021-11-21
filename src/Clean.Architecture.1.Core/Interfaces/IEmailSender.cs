using System.Threading.Tasks;

namespace Clean.Architecture._1.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string to, string from, string subject, string body);
}
