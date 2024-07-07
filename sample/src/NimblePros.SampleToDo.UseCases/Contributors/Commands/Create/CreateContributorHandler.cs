using Ardalis.Result;
using Ardalis.SharedKernel;
using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;

public class CreateContributorHandler : ICommandHandler<CreateContributorCommand, Result<int>>
{
  private readonly IRepository<Contributor> _repository;

  public CreateContributorHandler(IRepository<Contributor> repository)
  {
    _repository = repository;
  }

  public async Task<Result<int>> Handle(CreateContributorCommand request,
    CancellationToken cancellationToken)
  {
    var newContributor = new Contributor(request.Name);
    var createdItem = await _repository.AddAsync(newContributor, cancellationToken);

    return createdItem.Id;
  }
}
