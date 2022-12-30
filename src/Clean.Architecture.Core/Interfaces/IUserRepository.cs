using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clean.Architecture.Core.UserAggregate;

namespace Clean.Architecture.Core.Interfaces;
public interface IUserRepository
{
  Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
  Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken);
}
