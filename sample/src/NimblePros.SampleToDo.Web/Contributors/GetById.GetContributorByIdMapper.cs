using NimblePros.SampleToDo.UseCases.Contributors;

namespace NimblePros.SampleToDo.Web.Contributors;

public sealed class GetContributorByIdMapper
  : Mapper<GetContributorByIdRequest, ContributorRecord, ContributorDto>
{
  public override ContributorRecord FromEntity(ContributorDto e)
    => new(e.Id.Value, e.Name.Value);
}
