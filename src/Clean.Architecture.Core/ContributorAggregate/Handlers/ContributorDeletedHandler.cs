using Clean.Architecture.Core.ContributorAggregate.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.ContributorAggregate.Handlers;

public partial class ContributorDeletedHandler(ILogger<ContributorDeletedHandler> logger,
  IEmailSender emailSender) : INotificationHandler<ContributorDeletedEvent>
{
  private readonly ILogger<ContributorDeletedHandler> _logger = logger;

  [LoggerMessage(LogLevel.Information, "Handling Contributed Deleted event for {ContributorId}")]
  private partial void LogHandlingEvent(ContributorId contributorId);

  public async ValueTask Handle(ContributorDeletedEvent domainEvent, CancellationToken cancellationToken)
  {
    LogHandlingEvent(domainEvent.ContributorId);

    await emailSender.SendEmailAsync("to@test.com",
                                     "from@test.com",
                                     "Contributor Deleted",
                                     $"Contributor with id {domainEvent.ContributorId} was deleted.");
  }
}
