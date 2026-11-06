namespace Clean.Architecture.UseCases.Contributors.List;

public class ListContributorsHandler
  : IQueryHandler<ListContributorsQuery, Result<PagedResult<ContributorDto>>>
{
  private readonly IListContributorsQueryService _query;

  public ListContributorsHandler(IListContributorsQueryService query)
  {
    _query = query;
  }

  public async ValueTask<Result<PagedResult<ContributorDto>>> Handle(
    ListContributorsQuery request,
    CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(request);

    var result = await _query
      .ListAsync(
        request.Page ?? 1,
        request.PerPage ?? Constants.DEFAULT_PAGE_SIZE)
      .ConfigureAwait(false);

    return Result.Success(result);
  }
}
