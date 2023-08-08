using NimblePros.SampleToDo.Web.ContributorEndpoints;

namespace NimblePros.SampleToDo.Web.Endpoints.ContributorEndpoints;

public class ContributorListResponse
{
  public List<ContributorRecord> Contributors { get; set; } = new();
}
