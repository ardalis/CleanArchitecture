namespace NimblePros.SampleToDo.Infrastructure.Email;

public class MailserverConfiguration()
{
  public string Hostname { get; set; } = "localhost";
  public int Port { get; set; } = 25;
}
