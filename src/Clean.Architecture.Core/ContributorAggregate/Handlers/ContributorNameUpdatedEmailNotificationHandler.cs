using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.ContributorAggregate.Handlers;

public class ContributorNameUpdatedEmailNotificationHandler(
  ILogger<ContributorDeletedHandler> logger,
  IEmailSender emailSender) : INotificationHandler<ContributorNameUpdatedEvent>
{
  public async ValueTask Handle(ContributorNameUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Contributor Name Updated event for {contributorId}", domainEvent.Contributor.Id);

    await emailSender.SendEmailAsync("to@test.com",
                                     "from@test.com",
                                     $"Contributor {domainEvent.Contributor.Id} Name Updated",
$"Contributor with id {domainEvent.Contributor.Id} had their name updated to {domainEvent.Contributor.Name}.");
  }
}
