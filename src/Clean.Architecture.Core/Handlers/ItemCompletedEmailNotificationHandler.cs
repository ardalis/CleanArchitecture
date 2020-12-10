using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Clean.Architecture.Core.Events;
using Clean.Architecture.Core.Interfaces;
using MediatR;

namespace Clean.Architecture.Core.Handlers
{
    public class ItemCompletedEmailNotificationHandler : INotificationHandler<ToDoItemCompletedEvent>
    {
        private readonly IEmailSender _emailSender;

        public ItemCompletedEmailNotificationHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        // configure a test email server to demo this works
        // https://ardalis.com/configuring-a-local-test-email-server
        public Task Handle(ToDoItemCompletedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            return _emailSender.SendEmailAsync("test@test.com", "test@test.com", $"{domainEvent.CompletedItem.Title} was completed.", domainEvent.CompletedItem.ToString());
        }
    }
}
