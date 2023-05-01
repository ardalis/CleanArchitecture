namespace Clean.Architecture.Infrastructure;

public record MailserverConfiguration
{
  public string Hostname { get; set; } = null!;
  public int Port { get; set; }
}
