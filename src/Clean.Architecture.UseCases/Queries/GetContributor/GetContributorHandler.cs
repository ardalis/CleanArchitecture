using Ardalis.Result;
using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ContributorAggregate.Specifications;
using MediatR;

namespace Clean.Architecture.UseCases.Queries.GetContributor;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetContributorHandler : IRequestHandler<GetContributorCommand, Result<ContributorDTO>>
{
  private readonly IReadRepository<Contributor> _repository;

  public GetContributorHandler(IReadRepository<Contributor> repository)
  {
    _repository = repository;
  }

  public async Task<Result<ContributorDTO>> Handle(GetContributorCommand request, CancellationToken cancellationToken)
  {
    var spec = new ContributorByIdSpec(request.ContributorId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new ContributorDTO(entity.Id,entity.Name);
  }
}
