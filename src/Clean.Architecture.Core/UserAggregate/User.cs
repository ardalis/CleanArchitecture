using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Core.UserAggregate;
public class User : IdentityUser<int>, IAggregateRoot
{
  public User()
  {
    IsActive = true;
  }

  public string FullName { get; set; } = default!;

  public int Age { get; set; }

  public GenderType Gender { get; set; }

  public bool IsActive { get; set; }

  public DateTimeOffset? LastLoginDate { get; set; }
}

public enum GenderType
{
  Male = 1,
  Female = 2
}
