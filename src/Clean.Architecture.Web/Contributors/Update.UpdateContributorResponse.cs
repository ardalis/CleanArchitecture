using Clean.Architecture.Web.ContributorEndpoints;

namespace Clean.Architecture.Web.Endpoints.ContributorEndpoints;

public class UpdateContributorResponse(ContributorRecord contributor)
{
  public ContributorRecord Contributor { get; set; } = contributor;
}
