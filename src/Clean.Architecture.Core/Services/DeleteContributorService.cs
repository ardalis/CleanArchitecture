using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.Services;

/// <summary>
/// This is here mainly so there's an example of a domain service
/// and also to demonstrate how to fire domain events from a service.
/// </summary>
/// <param name="repository"></param>
/// <param name="mediator"></param>
/// <param name="logger"></param>
public partial class DeleteContributorService(IRepository<Contributor> repository,
  IMediator mediator,
  ILogger<DeleteContributorService> logger) : IDeleteContributorService
{
  private readonly IRepository<Contributor> _repository = repository;
  private readonly IMediator _mediator = mediator;
  private readonly ILogger<DeleteContributorService> _logger = logger;

  [LoggerMessage(LogLevel.Information, "Deleting Contributor {ContributorId}")]
  private partial void LogDeletingContributor(ContributorId contributorId);

  public async ValueTask<Result> DeleteContributor(ContributorId contributorId)
  {
    LogDeletingContributor(contributorId);
    Contributor? aggregateToDelete = await _repository.GetByIdAsync(contributorId);
    if (aggregateToDelete == null) return Result.NotFound();

    await _repository.DeleteAsync(aggregateToDelete);
    var domainEvent = new ContributorDeletedEvent(contributorId);
    await _mediator.Publish(domainEvent);

    return Result.Success();
  }
}
