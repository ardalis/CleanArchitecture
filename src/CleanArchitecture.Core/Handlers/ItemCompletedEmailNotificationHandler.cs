using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanArchitecture.Core.Events;
using CleanArchitecture.SharedKernel.Interfaces;

namespace CleanArchitecture.Core.Services
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
