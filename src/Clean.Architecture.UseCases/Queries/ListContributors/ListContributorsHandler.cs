using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Queries.GetContributor;

public interface IListContributorsQuery
{
  Task<IEnumerable<ContributorDTO>> ListAsync();
}

public class ListContributorsHandler : IRequestHandler<ListContributorsCommand, Result<IEnumerable<ContributorDTO>>>
{
  private readonly IListContributorsQuery _query;

  public ListContributorsHandler(IListContributorsQuery query)
  {
    _query = query;
  }

  public async Task<Result<IEnumerable<ContributorDTO>>> Handle(ListContributorsCommand request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}
