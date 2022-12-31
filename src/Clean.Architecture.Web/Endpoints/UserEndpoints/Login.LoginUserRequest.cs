using Microsoft.Build.Framework;

namespace Clean.Architecture.Web.Endpoints.UserEndpoints;

public class LoginUserRequest
{
  public const string Route = "/User/Login";
  [Required]
  public string Email { get; set; } = default!;

  [Required]
  public string Password { get; set; } = default!;

  [Required] 
  public string RefreshToken { get; set; } = default!;
}
