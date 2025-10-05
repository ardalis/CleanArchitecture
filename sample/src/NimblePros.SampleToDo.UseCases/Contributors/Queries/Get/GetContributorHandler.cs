using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ContributorAggregate.Specifications;

namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.Get;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetContributorHandler(IReadRepository<Contributor> repository)
  : IQueryHandler<GetContributorQuery, Result<ContributorDto>>
{
  private readonly IReadRepository<Contributor> _repository = repository;

  public async ValueTask<Result<ContributorDto>> Handle(GetContributorQuery request, CancellationToken cancellationToken)
  {
    var spec = new ContributorByIdSpec(request.ContributorId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new ContributorDto(entity.Id, entity.Name);
  }
}
