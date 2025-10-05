namespace NimblePros.SampleToDo.UseCases.Projects.ListShallow;

public class ListProjectsShallowHandler(IListProjectsShallowQueryService query)
  : IQueryHandler<ListProjectsShallowQuery, Result<IEnumerable<ProjectDto>>>
{
  private readonly IListProjectsShallowQueryService _query = query;

  public async ValueTask<Result<IEnumerable<ProjectDto>>> Handle(ListProjectsShallowQuery request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}
