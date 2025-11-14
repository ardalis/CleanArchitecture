namespace MinimalClean.Architecture.Web.Domain.GuestUserAggregate;

public class GuestUser : EntityBase<GuestUser, GuestUserId>, IAggregateRoot
{
  public GuestUser(GuestUserId id, string email)
  {
    Id = id;
    Email = email;
  }

  public string Email { get; private set; }

  public GuestUser UpdateEmail(string newEmail)
  {
    Email = newEmail;
    return this;
  }
}
