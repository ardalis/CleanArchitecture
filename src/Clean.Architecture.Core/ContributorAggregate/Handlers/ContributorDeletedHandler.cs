using Clean.Architecture.Core.ContributorAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clean.Architecture.Core.ContributorAggregate.Handlers;

/// <summary>
/// NOTE: Internal because ContributorDeleted is also marked as internal.
/// </summary>
internal class ContributorDeletedHandler(ILogger<ContributorDeletedHandler> _logger) : INotificationHandler<ContributorDeletedEvent>
{
  public async Task Handle(ContributorDeletedEvent domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Handling Contributed Deleted event for {contributorId}", domainEvent.ContributorId);

    // TODO: do meaningful work here
    await Task.Delay(1);
  }
}
