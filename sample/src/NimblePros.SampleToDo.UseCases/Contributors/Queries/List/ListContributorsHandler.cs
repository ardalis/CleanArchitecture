namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.List;

public class ListContributorsHandler : IQueryHandler<ListContributorsQuery, Result<IEnumerable<ContributorDTO>>>
{
  private readonly IListContributorsQueryService _query;

  public ListContributorsHandler(IListContributorsQueryService query)
  {
    _query = query;
  }

  public async ValueTask<Result<IEnumerable<ContributorDTO>>> Handle(ListContributorsQuery request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}
