using System.Security.Claims;
using System.Text;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.SharedKernel.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Clean.Architecture.Infrastructure.Identity;

namespace Clean.Architecture.Infrastructure;

public static class StartupSetup
{
  public static void AddDbContext(this IServiceCollection services, string connectionString) =>
      services.AddDbContext<AppDbContext>(options =>
          options.UseSqlite(connectionString)); // will be created in web project root

  public static void AddCustomJwtAuthentication(this IServiceCollection services, JwtSettings settings)
  {
    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
      .AddJwtBearer(options =>
      {
        var secretKey = Encoding.UTF8.GetBytes(settings.SecretKey);
        //You can uncomment encryptionKey and TokenDecryptionKey fields if you are going to use it
        //var encryptionKey = Encoding.UTF8.GetBytes(settings.EncryptKey);

        var validationParameters = new TokenValidationParameters
        {
          ClockSkew = TimeSpan.Zero, // default: 5 min
          RequireSignedTokens = true,

          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(secretKey),

          RequireExpirationTime = true,
          ValidateLifetime = true,

          ValidateAudience = true, //default : false
          ValidAudience = settings.Audience,

          ValidateIssuer = true, //default : false
          ValidIssuer = settings.Issuer,

          //TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
        };

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = validationParameters;

        options.Events = new JwtJwtBearerHelperEvents().GetJwtBeareEvents();
      });
  }

  public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings, string connectionString)
  {
    services.AddDbContext<AppIdentityDbContext>(options =>
        options.UseSqlServer(connectionString));
    services.AddIdentity<User, Role>(identityOptions =>
    {
      //Password Settings
      identityOptions.Password.RequireDigit = settings.PasswordRequireDigit;
      identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
      identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumeric; //#@!
      identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;
      identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;

      //UserName Settings
      identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;
    })
  .AddEntityFrameworkStores<AppIdentityDbContext>()
  .AddDefaultTokenProviders();
  }
}
/// <summary>
/// A helper JwtJwtBearerHelperEvents class which is added to make the code more readable and avoid any complexity 
/// </summary>
internal class JwtJwtBearerHelperEvents
{
  public JwtBearerEvents GetJwtBeareEvents()
  {
    return new JwtBearerEvents
    {
      OnAuthenticationFailed = OnAuthenticationFailedJwtBearerEvent(),
      OnTokenValidated = OnTokenValidatedJwtBearerEvent(),
      OnChallenge = OnOnChallengeJwtBearerEvent()
    };
  }
  private Func<AuthenticationFailedContext, Task> OnAuthenticationFailedJwtBearerEvent()
  {
    return (context) =>
    {
      if (context.Exception != null)
      {
        throw context.Exception;
      }
      return Task.CompletedTask;
    };
  }
  private Func<TokenValidatedContext, Task> OnTokenValidatedJwtBearerEvent()
  {
    return async context =>
    {
      var signInManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();

      var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
      if (claimsIdentity!.Claims?.Any() != true)
        context.Fail("This token has no claims.");

      //Get UserId and check if user exists in db
      var userId = claimsIdentity.GetUserId<int>();
      if (userId == 0)
        context.Fail("UserId has no value in claims.");
      var user = await signInManager.FindByIdAsync(userId.ToString());
      if (user == null)
        context.Fail("User not found.");

      if (!user!.IsActive)
        context.Fail("User is not active.");

      user.LastLoginDate = DateTime.Now;
      await signInManager.UpdateAsync(user);
    };
  }
  private Func<JwtBearerChallengeContext, Task> OnOnChallengeJwtBearerEvent()
  {
    return context =>
    {
      if (context.AuthenticateFailure != null)
        throw context.AuthenticateFailure;
      context.HandleResponse();
      context.Response.StatusCode = 401;
      context.Response.ContentType = "application/json";
      var result = JsonSerializer.Serialize(new { message = "You are not Authorized" });
      return context.Response.WriteAsync(result);
    };
  }
}


