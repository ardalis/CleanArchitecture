using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clean.Architecture.Infrastructure.Identity.Jwt;
public class JwtService : IJwtService
{
  private readonly SiteSettings _siteSetting;
  private readonly UserManager<User> _userManager;

  public JwtService(IOptionsSnapshot<SiteSettings> settings,
                    UserManager<User> userManager)
  {
    _siteSetting = settings.Value;
    _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
  }

  public async Task<AccessToken> GenerateAsync(User user)
  {
    var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
    var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

    //We can use EncryptingCredentials options in SecurityTokenDescriptor and hence our JWT token will not be parsed by jwt.io site and it will only be decrypted only by our code.
    //Hence you can secure your token and who can see it
    //var encryptionKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey); //must be 16 character
    //var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

    var claims = await GetClaimsAsync(user);

    var descriptor = new SecurityTokenDescriptor
    {
      Issuer = _siteSetting.JwtSettings.Issuer,
      Audience = _siteSetting.JwtSettings.Audience,
      IssuedAt = DateTime.Now,
      NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
      Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
      SigningCredentials = signingCredentials,
      //EncryptingCredentials = encryptingCredentials,
      Subject = new ClaimsIdentity(claims)
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

    return new AccessToken(securityToken: securityToken);
  }

  public int? ValidateJwtAccessTokenAsync(string token)
  {
    var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character

    //if you are giving a value to EncryptingCredentials while generating a token then uncomment the encryptionKey and TokenDecryptionKey option so token can be parsed while validating. 
    //var encryptionKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey); //must be 16 character

    var tokenHandler = new JwtSecurityTokenHandler();
    try
    {
      tokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        //TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey),
        ValidateIssuer = false,
        ValidateAudience = false,
        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        ClockSkew = TimeSpan.Zero
      }, out var validatedToken);

      var jwtSecurityToken = (JwtSecurityToken)validatedToken;
      var userId = int.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value);
      return userId;
    }
    catch
    {
      throw;
    }
  }

  private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
  {
    var claims = new List<Claim>();
    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
    claims.Add(new Claim(ClaimTypes.Name, user.UserName!));

    var userRoles = await _userManager.GetRolesAsync(user);

    foreach (var role in userRoles)
    {
      claims.Add(new Claim(ClaimTypes.Role, role));
    }

    return claims;
  }
}
