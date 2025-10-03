using NimblePros.SampleToDo.Core.ContributorAggregate.Events;

namespace NimblePros.SampleToDo.Core.ContributorAggregate.Handlers;

internal class ContributorNameUpdatedEventLoggingHandler(ILogger<ContributorNameUpdatedEventLoggingHandler> logger) :
  Mediator.INotificationHandler<ContributorNameUpdatedEvent>
{
  private readonly ILogger<ContributorNameUpdatedEventLoggingHandler> _logger = logger;

  public ValueTask Handle(ContributorNameUpdatedEvent notification, CancellationToken cancellationToken)
  {
    var contributorId = notification.Contributor.Id;
    string newName = notification.Contributor.Name.Value;
    _logger.LogInformation("Contributor {contributorId}'s name was updated to {newName}", contributorId, newName);
    return ValueTask.CompletedTask;
  }
}
