using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Clean.Architecture.Core.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.Services
{
    public class ItemCompletedEmailNotificationHandler : IHandle<ToDoItemCompletedEvent>
    {
        public Task Handle(ToDoItemCompletedEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            // Do Nothing

            return Task.CompletedTask;
        }
    }
}
