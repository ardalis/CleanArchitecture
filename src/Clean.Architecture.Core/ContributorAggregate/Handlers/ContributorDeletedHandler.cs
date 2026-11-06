using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.ContributorAggregate.Handlers;

public partial class ContributorDeletedHandler(
  ILogger<ContributorDeletedHandler> logger,
  IEmailSender emailSender) : INotificationHandler<ContributorDeletedEvent>
{
  public async ValueTask Handle(
    ContributorDeletedEvent domainEvent,
    CancellationToken cancellationToken)
  {
    Guard.Against.Null(domainEvent);

    LogHandlingContributorDeleted(logger, domainEvent.ContributorId);

    await emailSender.SendEmailAsync(
        "to@test.com",
        "from@test.com",
        "Contributor Deleted",
        $"Contributor with id {domainEvent.ContributorId} was deleted.")
      .ConfigureAwait(false);
  }

  [LoggerMessage(
    EventId = 1,
    Level = LogLevel.Information,
    Message = "Handling Contributor Deleted event for {ContributorId}")]
  private static partial void LogHandlingContributorDeleted(
    ILogger<ContributorDeletedHandler> logger,
    ContributorId contributorId);
}
