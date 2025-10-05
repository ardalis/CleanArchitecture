namespace NimblePros.SampleToDo.Infrastructure.Email;

public class MailserverConfiguration()
{
  public const string SectionName = "Mailserver";

  public string Hostname { get; set; } = "localhost";
  public int Port { get; set; } = 25;
}
