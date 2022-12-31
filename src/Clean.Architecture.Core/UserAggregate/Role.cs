using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Core.UserAggregate;
public class Role : IdentityRole<int>, IAggregateRoot
{
  public string Description { get; set; } = default!;
}
