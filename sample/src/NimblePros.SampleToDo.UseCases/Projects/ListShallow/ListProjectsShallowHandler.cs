namespace NimblePros.SampleToDo.UseCases.Projects.ListShallow;

public class ListProjectsShallowHandler(IListProjectsShallowQueryService query)
  : IQueryHandler<ListProjectsShallowQuery, Result<IEnumerable<ProjectDTO>>>
{
  private readonly IListProjectsShallowQueryService _query = query;

  public async ValueTask<Result<IEnumerable<ProjectDTO>>> Handle(ListProjectsShallowQuery request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}
