using MailKit.Security;

namespace Clean.Architecture.Infrastructure.Email;

public class MailserverConfiguration()
{
  public string Hostname { get; set; } = "localhost";
  public int Port { get; set; } = 25;

  /// <summary>
  /// Optional SMTP credentials. When Username is set, the sender authenticates before sending.
  /// Store the password in user secrets or your host's secret store, not in appsettings.json.
  /// </summary>
  public string? Username { get; set; }
  public string? Password { get; set; }

  /// <summary>
  /// Requires STARTTLS by default so mail and credentials are never sent in cleartext.
  /// Local test servers such as Papercut don't support TLS; appsettings.Development.json sets this to None.
  /// </summary>
  public SecureSocketOptions SecureSocketOptions { get; set; } = SecureSocketOptions.StartTls;
}
