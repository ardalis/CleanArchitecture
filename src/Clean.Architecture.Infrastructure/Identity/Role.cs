using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Infrastructure.Identity;
public class Role : IdentityRole<int>, IAggregateRoot
{
  public string Description { get; set; } = default!;
}
