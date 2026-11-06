using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.UseCases.Contributors.Update;

public class UpdateContributorHandler(
  IRepository<Contributor> repository)
  : ICommandHandler<UpdateContributorCommand, Result<ContributorDto>>
{
  public async ValueTask<Result<ContributorDto>> Handle(
    UpdateContributorCommand command,
    CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(command);

    var existingContributor = await repository
      .GetByIdAsync(command.ContributorId, cancellationToken)
      .ConfigureAwait(false);

    if (existingContributor is null)
    {
      return Result.NotFound();
    }

    existingContributor.UpdateName(command.NewName);

    await repository
      .UpdateAsync(existingContributor, cancellationToken)
      .ConfigureAwait(false);

    return new ContributorDto(
      existingContributor.Id,
      existingContributor.Name,
      existingContributor.PhoneNumber ?? PhoneNumber.Unknown);
  }
}
