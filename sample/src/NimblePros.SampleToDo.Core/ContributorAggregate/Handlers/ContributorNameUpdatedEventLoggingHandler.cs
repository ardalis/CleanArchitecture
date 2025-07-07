using NimblePros.SampleToDo.Core.ContributorAggregate.Events;

namespace NimblePros.SampleToDo.Core.ContributorAggregate.Handlers;

internal class ContributorNameUpdatedEventLoggingHandler(ILogger<ContributorNameUpdatedEventLoggingHandler> logger) : INotificationHandler<ContributorNameUpdatedEvent>
{
  private readonly ILogger<ContributorNameUpdatedEventLoggingHandler> _logger = logger;

  public Task Handle(ContributorNameUpdatedEvent notification, CancellationToken cancellationToken)
  {
    int contributorId = notification.Contributor.Id;
    string newName = notification.Contributor.Name.Value;
    _logger.LogInformation("Contributor {contributorId}'s name was updated to {newName}", contributorId, newName);
    return Task.CompletedTask;
  }
}
