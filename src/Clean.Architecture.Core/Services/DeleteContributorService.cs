using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.Services;

/// <summary>
/// This is here mainly so there's an example of a domain service
/// and also to demonstrate how to fire domain events from a service.
/// </summary>
/// <param name="repository">The repository used to load and delete contributors.</param>
/// <param name="mediator">The mediator used to publish domain events.</param>
/// <param name="logger">The logger used by the service.</param>
public class DeleteContributorService(
  IRepository<Contributor> repository,
  IMediator mediator,
  ILogger<DeleteContributorService> logger) : IDeleteContributorService
{
  private static readonly Action<ILogger, ContributorId, Exception?> LogDeletingContributor =
    LoggerMessage.Define<ContributorId>(
      LogLevel.Information,
      new EventId(1, nameof(DeleteContributor)),
      "Deleting Contributor {ContributorId}");

  private readonly IRepository<Contributor> _repository = repository;
  private readonly IMediator _mediator = mediator;
  private readonly ILogger<DeleteContributorService> _logger = logger;

  public async ValueTask<Result> DeleteContributor(ContributorId contributorId)
  {
    LogDeletingContributor(_logger, contributorId, null);

    Contributor? aggregateToDelete = await _repository.GetByIdAsync(contributorId);
    if (aggregateToDelete == null) return Result.NotFound();

    await _repository.DeleteAsync(aggregateToDelete);

    var domainEvent = new ContributorDeletedEvent(contributorId);
    await _mediator.Publish(domainEvent);
    return Result.Success();
  }
}
