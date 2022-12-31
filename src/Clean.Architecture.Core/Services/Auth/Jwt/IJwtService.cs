using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clean.Architecture.Core.UserAggregate;

namespace Clean.Architecture.Core.Services.Auth.Jwt;
public interface IJwtService
{
  Task<AccessToken> GenerateAsync(User user);
  int? ValidateJwtAccessTokenAsync(string token);
}
