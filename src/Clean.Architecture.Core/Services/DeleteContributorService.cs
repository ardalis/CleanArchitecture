using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.Services;

/// <summary>
/// This is here mainly so there's an example of a domain service
/// and also to demonstrate how to fire domain events from a service.
/// </summary>
public partial class DeleteContributorService(
  IRepository<Contributor> repository,
  IMediator mediator,
  ILogger<DeleteContributorService> logger) : IDeleteContributorService
{
  public async ValueTask<Result> DeleteContributor(
    ContributorId contributorId)
  {
    LogDeletingContributor(logger, contributorId);

    Contributor? aggregateToDelete = await repository
      .GetByIdAsync(contributorId)
      .ConfigureAwait(false);

    if (aggregateToDelete is null)
    {
      return Result.NotFound();
    }

    await repository
      .DeleteAsync(aggregateToDelete)
      .ConfigureAwait(false);

    var domainEvent = new ContributorDeletedEvent(contributorId);

    await mediator
      .Publish(domainEvent)
      .ConfigureAwait(false);

    return Result.Success();
  }

  [LoggerMessage(
    EventId = 3,
    Level = LogLevel.Information,
    Message = "Deleting Contributor {ContributorId}")]
  private static partial void LogDeletingContributor(
    ILogger<DeleteContributorService> logger,
    ContributorId contributorId);
}
