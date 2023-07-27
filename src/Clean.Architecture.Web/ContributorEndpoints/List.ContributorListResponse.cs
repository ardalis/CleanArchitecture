using Clean.Architecture.Web.ContributorEndpoints;

namespace Clean.Architecture.Web.Endpoints.ContributorEndpoints;

public class ContributorListResponse
{
  public List<ContributorRecord> Contributors { get; set; } = new();
}
