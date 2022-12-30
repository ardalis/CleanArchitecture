using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.UserAggregate;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Infrastructure;
public class UserRepository : IUserRepository
{
  readonly IRepository<User> _repository;
  public UserRepository(IRepository<User> repository)
  {
    _repository = repository;
  }

  public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken)
  {
    return await _repository.GetByIdAsync(userId, cancellationToken);
  }

  public async Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
  {
    user.LastLoginDate = DateTime.Now;
    await _repository.UpdateAsync(user, cancellationToken);
  }
}
