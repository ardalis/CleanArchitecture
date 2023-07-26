using Ardalis.Result;
using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;
using MediatR;

namespace Clean.Architecture.UseCases.Commands.CreateContributor;

public class CreateContributorHandler : IRequestHandler<CreateContributorCommand, Result>
{
  private readonly IRepository<Contributor> _repository;
  private readonly IMediator _mediator;

  public CreateContributorHandler(IRepository<Contributor> repository,
    IMediator mediator)
  {
    _repository = repository;
    _mediator = mediator;
  }

  public async Task<Result> Handle(CreateContributorCommand request, CancellationToken cancellationToken)
  {
    // OPTION 1: Do the real work here including dispatching domain events - change the event from internal to public
    //var aggregateToDelete = await _repository.GetByIdAsync(request.ContributorId);
    //if (aggregateToDelete == null) return Result.NotFound();

    //await _repository.DeleteAsync(aggregateToDelete);
    // var domainEvent = new ContributorDeletedEvent(request.ContributorId);
    // await _mediator.Publish(null);
    await Task.Delay(1);
    return Result.Success();

    // OPTION 2: Keep Domain Events in the Domain Model / Core project; this becomes a pass-through
    //return await _deleteContributorService.DeleteContributor(request.ContributorId);
  }
}
