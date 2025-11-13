namespace MinimalClean.Architecture.Web.Domain.GuestUserAggregate.Specifications;

public class GuestUserByEmailSpec : Specification<GuestUser>
{
  public GuestUserByEmailSpec(string email) =>
    Query.Where(g => g.Email == email);
}