namespace Clean.Architecture.Web.Endpoints.UserEndpoints;

public class LoginUserResponse
{
  public string AccessToken { get; set; } = default!;
  public string TokenType { get; set; } = default!;
  public int ExpiresIn { get; set; }
}
