using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.ContributorAggregate.Handlers;

public partial class ContributorNameUpdatedEmailNotificationHandler(
  ILogger<ContributorNameUpdatedEmailNotificationHandler> logger,
  IEmailSender emailSender) : INotificationHandler<ContributorNameUpdatedEvent>
{
  private readonly ILogger<ContributorNameUpdatedEmailNotificationHandler> _logger = logger;

  [LoggerMessage(LogLevel.Information, "Handling Contributor Name Updated event for {ContributorId}")]
  private partial void LogHandlingEvent(ContributorId contributorId);

  public async ValueTask Handle(ContributorNameUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    LogHandlingEvent(domainEvent.Contributor.Id);

    await emailSender.SendEmailAsync("to@test.com",
                                     "from@test.com",
                                     $"Contributor {domainEvent.Contributor.Id} Name Updated",
$"Contributor with id {domainEvent.Contributor.Id} had their name updated to {domainEvent.Contributor.Name}.");
  }
}
