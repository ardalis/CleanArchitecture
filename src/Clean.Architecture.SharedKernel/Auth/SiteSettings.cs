using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.SharedKernel.Auth;
public class SiteSettings
{
  public JwtSettings JwtSettings { get; set; } = default!;
  public IdentitySettings IdentitySettings { get; set; } = default!;
}

public class IdentitySettings
{
  public bool PasswordRequireDigit { get; set; }
  public int PasswordRequiredLength { get; set; }
  public bool PasswordRequireNonAlphanumeric { get; set; }
  public bool PasswordRequireUppercase { get; set; }
  public bool PasswordRequireLowercase { get; set; }
  public bool RequireUniqueEmail { get; set; }
}

public class JwtSettings
{
  public string SecretKey { get; set; } = default!;
  public string EncryptKey { get; set; } = default!;
  public string Issuer { get; set; } = default!;
  public string Audience { get; set; } = default!;
  public int NotBeforeMinutes { get; set; }
  public int ExpirationMinutes { get; set; }
  public int RefreshTokenValidityInDays { get; set; }
}
