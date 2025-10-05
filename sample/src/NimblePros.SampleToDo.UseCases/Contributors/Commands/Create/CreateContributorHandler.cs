using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;

public class CreateContributorHandler(IRepository<Contributor> repository)
  : ICommandHandler<CreateContributorCommand, Result<int>>
{
  private readonly IRepository<Contributor> _repository = repository;

  public async ValueTask<Result<int>> Handle(CreateContributorCommand command, CancellationToken cancellationToken)
  {
    var newContributor = new Contributor(command.Name);
    var createdItem = await _repository.AddAsync(newContributor, cancellationToken);

    return createdItem.Id.Value;
  }
}
