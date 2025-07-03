using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.UseCases.Contributors.Create;
using FastEndpoints;

namespace Clean.Architecture.UseCases.Contributors.List;

public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDTO>>>;
public record ListContributorsQuery2(int? Skip, int? Take) : FastEndpoints.ICommand<Result<IEnumerable<ContributorDTO>>>;

public class ListContributorsQueryHandler2 : CommandHandler<ListContributorsQuery2, Result<IEnumerable<ContributorDTO>>>
{
  private readonly IListContributorsQueryService _query;

  public ListContributorsQueryHandler2(IListContributorsQueryService query)
  {
    _query = query;
  }
  public override async Task<Result<IEnumerable<ContributorDTO>>> ExecuteAsync(ListContributorsQuery2 request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    Console.WriteLine($"<<<<<<<Listed {result.Count()} contributors");

    return Result.Success(result);
  }
}
