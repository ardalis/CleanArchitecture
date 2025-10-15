using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.UseCases.Contributors.Update;

public class UpdateContributorHandler(IRepository<Contributor> _repository)
  : ICommandHandler<UpdateContributorCommand, Result<ContributorDto>>
{
  public async ValueTask<Result<ContributorDto>> Handle(UpdateContributorCommand command, 
    CancellationToken ct)
  {
    var existingContributor = await _repository.GetByIdAsync(command.ContributorId, ct);
    if (existingContributor == null)
    {
      return Result.NotFound();
    }

    existingContributor.UpdateName(command.NewName);

    await _repository.UpdateAsync(existingContributor, ct);

    return new ContributorDto(existingContributor.Id,
      existingContributor.Name, existingContributor.PhoneNumber ?? PhoneNumber.Unknown);
  }
}
