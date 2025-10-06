using Microsoft.Extensions.Logging;
using NimblePros.SampleToDo.Core.ProjectAggregate.Events;

namespace NimblePros.SampleToDo.Core.ProjectAggregate.Handlers;

public class ContributorAddedToItemLoggingHandler(ILogger<ContributorAddedToItemLoggingHandler> logger)
  : INotificationHandler<ContributorAddedToItemEvent>
{
  private readonly ILogger<ContributorAddedToItemLoggingHandler> _logger = logger;

  public ValueTask Handle(ContributorAddedToItemEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Contributor {ContributorId} assigned to ToDo item {Item}", notification.ContributorId,  notification.Item);
    return default;
  }
}
