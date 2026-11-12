using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ContributorAggregate.Specifications;

namespace Clean.Architecture.UseCases.Contributors.GetById;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetContributorByIdHandler(IReadRepository<Contributor> _repository)
  : IQueryHandler<GetContributorByIdQuery, Result<ContributorDto>>
{
  public async ValueTask<Result<ContributorDto>> Handle(GetContributorByIdQuery request, CancellationToken cancellationToken)
  {
    var spec = new ContributorByIdSpec(request.ContributorId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new ContributorDto(entity.Id, entity.Name, entity.PhoneNumber ?? PhoneNumber.Unknown);
  }
}
