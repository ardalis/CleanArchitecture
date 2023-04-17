using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Infrastructure.Identity.Jwt;
public interface IJwtService
{
  Task<AccessToken> GenerateAsync(User user);
  int? ValidateJwtAccessTokenAsync(string token);
}
