using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.ContributorAggregate.Handlers;

public partial class ContributorNameUpdatedEmailNotificationHandler(
  ILogger<ContributorNameUpdatedEmailNotificationHandler> logger,
  IEmailSender emailSender) : INotificationHandler<ContributorNameUpdatedEvent>
{
  public async ValueTask Handle(
    ContributorNameUpdatedEvent domainEvent,
    CancellationToken cancellationToken)
  {
    Guard.Against.Null(domainEvent);

    LogHandlingContributorNameUpdated(
      logger,
      domainEvent.Contributor.Id);

    await emailSender.SendEmailAsync(
        "to@test.com",
        "from@test.com",
        $"Contributor {domainEvent.Contributor.Id} Name Updated",
        $"Contributor with id {domainEvent.Contributor.Id} had their name updated to {domainEvent.Contributor.Name}.")
      .ConfigureAwait(false);
  }

  [LoggerMessage(
    EventId = 2,
    Level = LogLevel.Information,
    Message = "Handling Contributor Name Updated event for {ContributorId}")]
  private static partial void LogHandlingContributorNameUpdated(
    ILogger<ContributorNameUpdatedEmailNotificationHandler> logger,
    ContributorId contributorId);
}
