using MartiX.Clean.Architecture.Web.Domain.GuestUserAggregate;
using MartiX.Clean.Architecture.Web.Domain.GuestUserAggregate.Specifications;
using MartiX.Clean.Architecture.Web.Infrastructure.Data;

namespace MartiX.Clean.Architecture.Web.Tests;

public class GuestUserDomainTests
{
  [Test]
  public async Task UpdateEmail_WhenCalled_UpdatesEmail()
  {
    var user = new GuestUser(GuestUserId.From(Guid.NewGuid()), "before@test.dev");

    user.UpdateEmail("after@test.dev");

    await Assert.That(user.Email != "after@test.dev").IsFalse();
  }

  [Test]
  public async Task FirstOrDefaultAsync_WhenGuestUserIdMatches_ReturnsExpectedUser()
  {
    await using var db = TestDbContextHelper.CreateInMemoryAppDbContext();
    var repo = new EfRepository<GuestUser>(db);
    var expected = new GuestUser(GuestUserId.From(Guid.NewGuid()), "expected@test.dev");
    await repo.AddAsync(new GuestUser(GuestUserId.From(Guid.NewGuid()), "other@test.dev"), CancellationToken.None);
    await repo.AddAsync(expected, CancellationToken.None);
    await repo.SaveChangesAsync(CancellationToken.None);

    var found = await repo.FirstOrDefaultAsync(new GuestUserByIdSpec(expected.Id), CancellationToken.None);

    await Assert.That(found is null || found.Id != expected.Id).IsFalse();
  }
}

