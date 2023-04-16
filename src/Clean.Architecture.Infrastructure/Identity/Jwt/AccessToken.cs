using System.IdentityModel.Tokens.Jwt;

namespace Clean.Architecture.Infrastructure.Identity.Jwt;
public class AccessToken
{
  public string Access_Token { get; set; }
  public string TokenType { get; set; }
  public int ExpiresIn { get; set; }

  public AccessToken(JwtSecurityToken securityToken)
  {
    Access_Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
    TokenType = "Bearer";
    ExpiresIn = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
  }
}
