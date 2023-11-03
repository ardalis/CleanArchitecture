using Ardalis.Result;
using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.UseCases.Contributors.Update;

public class UpdateContributorHandler(IRepository<Contributor> _repository)
  : ICommandHandler<UpdateContributorCommand, Result<ContributorDTO>>
{
  public async Task<Result<ContributorDTO>> Handle(UpdateContributorCommand request, CancellationToken cancellationToken)
  {
    var existingContributor = await _repository.GetByIdAsync(request.ContributorId, cancellationToken);
    if (existingContributor == null)
    {
      return Result.NotFound();
    }

    existingContributor.UpdateName(request.NewName!);

    await _repository.UpdateAsync(existingContributor, cancellationToken);

    return Result.Success(new ContributorDTO(existingContributor.Id, existingContributor.Name));
  }
}
