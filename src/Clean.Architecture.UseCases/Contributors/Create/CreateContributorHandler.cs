using Ardalis.Result;
using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.UseCases.Contributors.Create;

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
