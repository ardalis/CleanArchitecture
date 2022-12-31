using Clean.Architecture.Core.UserAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Web.Endpoints.UserEndpoints;

public class SignUp : Endpoint<SignUpUserRequest>
{
  private readonly UserManager<User> _userManager;
  private readonly RoleManager<Role> _roleManager;
  public SignUp(UserManager<User> userManager, RoleManager<Role> roleManager)
  {
    _userManager = userManager;
    _roleManager = roleManager;
  }

  public override void Configure()
  {
    Post(SignUpUserRequest.Route);
    AllowAnonymous();
    Options(x => x
      .WithTags("UserEndpoints"));
  }
  public override async Task HandleAsync(
    SignUpUserRequest request,
    CancellationToken cancellationToken)
  {
    if (request is null)
      ThrowError("Request body is empty");
    //TODO:CHECK FOR USERNAME EMPTY OR NULL
    var newUser = new User
    {
      Age = request.Age,
      FullName = request.FullName!,
      Gender = request.Gender,
      UserName = request.UserName,
      Email = request.Email
    };
    var createUserResult = await _userManager.CreateAsync(newUser, request.Password);
    if (!createUserResult.Succeeded) { ThrowValidationErrors(createUserResult.Errors); }

    var role = await _roleManager.FindByNameAsync("Admin");
    if (role==null)
    {
      var addRoleResult = await _roleManager.CreateAsync(new Role
      {
        Name = "Admin",
        Description = "admin role"
      });
      if (!addRoleResult.Succeeded) { ThrowValidationErrors(addRoleResult.Errors); }
    }
    else
    {
      var assignRoleResult = await _userManager.AddToRoleAsync(newUser, role.Name!);
      if (!assignRoleResult.Succeeded) { ThrowValidationErrors(assignRoleResult.Errors); }
    }

    await SendNoContentAsync();
  }
  private void ThrowValidationErrors(IEnumerable<IdentityError> identityErrors)
  {
    foreach (var error in identityErrors)
    {
      AddError(error.Description, error.Code);
    }
    ThrowIfAnyErrors();
  }
}

