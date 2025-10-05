
namespace NimblePros.SampleToDo.Web.Contributors;

public record ContributorListResponse : UseCases.PagedResult<ContributorRecord>
{
  public ContributorListResponse(IReadOnlyList<ContributorRecord> Items, int Page, int PerPage, int TotalCount, int TotalPages)
    : base(Items, Page, PerPage, TotalCount, TotalPages)
  {
  }
}
