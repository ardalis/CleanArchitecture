using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ContributorAggregate.Specifications;

namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Update;

public class UpdateContributorHandler : ICommandHandler<UpdateContributorCommand, Result<ContributorDTO>>
{
  private readonly IRepository<Contributor> _repository;

  public UpdateContributorHandler(IRepository<Contributor> repository)
  {
    _repository = repository;
  }

  public async ValueTask<Result<ContributorDTO>> Handle(UpdateContributorCommand request, CancellationToken cancellationToken)
  {
    var spec = new ContributorByIdSpec(request.ContributorId);
    var existingContributor = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (existingContributor == null)
    {
      return Result.NotFound();
    }

    existingContributor.UpdateName(request.NewName!);

    await _repository.UpdateAsync(existingContributor, cancellationToken);

    return Result.Success(new ContributorDTO(existingContributor.Id, existingContributor.Name));
  }
}
