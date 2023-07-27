using Ardalis.Result;
using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;
using MediatR;

namespace Clean.Architecture.UseCases.Commands.CreateContributor;

public class CreateContributorHandler : IRequestHandler<CreateContributorCommand, Result<int>>
{
  private readonly IRepository<Contributor> _repository;
  private readonly IMediator _mediator;

  public CreateContributorHandler(IRepository<Contributor> repository,
    IMediator mediator)
  {
    _repository = repository;
    _mediator = mediator;
  }

  public async Task<Result<int>> Handle(CreateContributorCommand request, CancellationToken cancellationToken)
  {
    var newContributor = new Contributor(request.Name);
    var createdItem = await _repository.AddAsync(newContributor, cancellationToken);

    return createdItem.Id;
  }
}
