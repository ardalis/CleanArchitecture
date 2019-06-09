using Ardalis.GuardClauses;
using Clean.Architecture.Core.Events;
using Clean.Architecture.Core.Interfaces;

namespace Clean.Architecture.Core.Services
{
    public class ToDoItemService : IHandle<ToDoItemCompletedEvent>
    {
        public void Handle(ToDoItemCompletedEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            // Do Nothing
        }
    }
}
