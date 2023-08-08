using Clean.Architecture.Core.ContributorAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clean.Architecture.Core.ContributorAggregate.Handlers;

/// <summary>
/// NOTE: Internal because ContributorDeleted is also marked as internal.
/// </summary>
internal class ContributorDeletedHandler : INotificationHandler<ContributorDeletedEvent>
{
  private readonly ILogger<ContributorDeletedHandler> _logger;

  public ContributorDeletedHandler(ILogger<ContributorDeletedHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(ContributorDeletedEvent domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Handling Contributed Deleted event for {contributorId}", domainEvent.ContributorId);

    // TODO: do meaningful work here
    await Task.Delay(1);
  }
}
