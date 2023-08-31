using NimblePros.SampleToDo.Web.Contributors;

namespace NimblePros.SampleToDo.Web.Contributors;

public class ContributorListResponse
{
  public List<ContributorRecord> Contributors { get; set; } = new();
}
