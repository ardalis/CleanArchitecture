using Clean.Architecture.Infrastructure.Identity;
using Clean.Architecture.Infrastructure.Identity.Jwt;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Web.Endpoints.UserEndpoints;

public class Login : Endpoint<LoginUserRequest>
{
  private readonly UserManager<User> _userManager;
  private readonly IJwtService _jwtService;
  public Login(UserManager<User> userManager, IJwtService jwtService)
  {
    _userManager = userManager;
    _jwtService = jwtService;
  }

  public override void Configure()
  {
    Post(LoginUserRequest.Route);
    AllowAnonymous();
    Options(x => x
      .WithTags("UserEndpoints"));
  }
  public override async Task HandleAsync(
    LoginUserRequest request,
    CancellationToken cancellationToken)
  {
    if (request is null)
      ThrowError("Request body is empty");
    var user = await _userManager.FindByEmailAsync(request.Email); //userName/email can be used to find a unique user
    if (user == null)
      ThrowError("username or password is incorrect");

    var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
    if (!isPasswordValid)
      ThrowError("username or password is incorrect");

    var jwt = await _jwtService.GenerateAsync(user);
    user.LastLoginDate = DateTime.UtcNow;
    await _userManager.UpdateAsync(user);
    await SendOkAsync(new LoginUserResponse
    {
      AccessToken = jwt.Access_Token,
      TokenType = jwt.TokenType,
      ExpiresIn = jwt.ExpiresIn
    });
  }
}
