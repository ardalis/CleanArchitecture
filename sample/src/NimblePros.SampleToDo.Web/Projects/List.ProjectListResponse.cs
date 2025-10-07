namespace NimblePros.SampleToDo.Web.Projects;

public record ProjectListResponse : UseCases.PagedResult<ProjectRecord>
{
  public ProjectListResponse(IReadOnlyList<ProjectRecord> Items, int Page, int PerPage, int TotalCount, int TotalPages)
    : base(Items, Page, PerPage, TotalCount, TotalPages)
  {
  }
}
