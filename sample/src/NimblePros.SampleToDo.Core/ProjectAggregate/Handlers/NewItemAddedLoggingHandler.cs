using Microsoft.Extensions.Logging;
using NimblePros.SampleToDo.Core.ProjectAggregate.Events;

namespace NimblePros.SampleToDo.Core.ProjectAggregate.Handlers;

public class NewItemAddedLoggingHandler(ILogger<NewItemAddedLoggingHandler> logger)
  : INotificationHandler<NewItemAddedEvent>
{
  private readonly ILogger<NewItemAddedLoggingHandler> _logger = logger;

  public ValueTask Handle(NewItemAddedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("New Item {Item} added to Project {Project}", notification.NewItem,  notification.Project);
    return default;
  }
}
