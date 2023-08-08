using NimblePros.SampleToDo.Web.ContributorEndpoints;

namespace NimblePros.SampleToDo.Web.Endpoints.ContributorEndpoints;

public class UpdateContributorResponse
{
  public UpdateContributorResponse(ContributorRecord contributor)
  {
    Contributor = contributor;
  }
  public ContributorRecord Contributor { get; set; }
}
