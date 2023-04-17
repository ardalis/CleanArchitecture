using System.ComponentModel.DataAnnotations;
using Clean.Architecture.Infrastructure.Identity;

namespace Clean.Architecture.Web.Endpoints.UserEndpoints;

public class SignUpUserRequest
{
  public const string Route = "/User/SignUp";

  [Required]
  public string UserName { get; set; } = default!;

  [Required]
  public string Email { get; set; } = default!;


  [Required]
  public string Password { get; set; } = default!;

  [Required]
  public string FullName { get; set; } = default!;

  [Required]
  public int Age { get; set; }

  [Required]
  public GenderType Gender { get; set; }
}
