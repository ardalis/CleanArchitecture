using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ContributorAggregate.Specifications;
// CA1716 flags the namespace segment "Get" because it conflicts with a
// reserved keyword in some .NET languages. Renaming the namespace changes
// the public namespace and requires all references to be updated.
// The permanent fix is tracked separately in issue #1065.
#pragma warning disable CA1716 // Preserve the established public namespace.
namespace Clean.Architecture.UseCases.Contributors.Get;
#pragma warning restore CA1716

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient.
/// </summary>
public class GetContributorHandler(
  IReadRepository<Contributor> repository)
  : IQueryHandler<GetContributorQuery, Result<ContributorDto>>
{
  public async ValueTask<Result<ContributorDto>> Handle(
    GetContributorQuery request,
    CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(request);

    var spec = new ContributorByIdSpec(request.ContributorId);

    var entity = await repository
      .FirstOrDefaultAsync(spec, cancellationToken)
      .ConfigureAwait(false);

    if (entity is null)
    {
      return Result.NotFound();
    }

    return new ContributorDto(
      entity.Id,
      entity.Name,
      entity.PhoneNumber ?? PhoneNumber.Unknown);
  }
}
