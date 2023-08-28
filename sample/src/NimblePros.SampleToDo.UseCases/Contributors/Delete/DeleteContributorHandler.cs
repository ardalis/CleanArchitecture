using Ardalis.Result;
using Ardalis.SharedKernel;
using NimblePros.SampleToDo.Core.Interfaces;

namespace NimblePros.SampleToDo.UseCases.Contributors.Delete;

public class DeleteContributorHandler : ICommandHandler<DeleteContributorCommand, Result>
{
  private readonly IDeleteContributorService _deleteContributorService;

  public DeleteContributorHandler(IDeleteContributorService deleteContributorService)
  {
    _deleteContributorService = deleteContributorService;
  }

  public async Task<Result> Handle(DeleteContributorCommand request, CancellationToken cancellationToken)
  {
    // This Approach: Keep Domain Events in the Domain Model / Core project; this becomes a pass-through
    return await _deleteContributorService.DeleteContributor(request.ContributorId);

    // Another Approach: Do the real work here including dispatching domain events - change the event from internal to public
    // Ardalis prefers using the service so that "domain event" behavior remains in the domain model / core project
    // var aggregateToDelete = await _repository.GetByIdAsync(request.ContributorId);
    // if (aggregateToDelete == null) return Result.NotFound();

    // await _repository.DeleteAsync(aggregateToDelete);
    // var domainEvent = new ContributorDeletedEvent(request.ContributorId);
    // await _mediator.Publish(domainEvent);
    // return Result.Success();
  }
}
