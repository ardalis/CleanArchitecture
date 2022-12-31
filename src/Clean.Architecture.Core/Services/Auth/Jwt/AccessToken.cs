using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Services.Auth.Jwt;
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
