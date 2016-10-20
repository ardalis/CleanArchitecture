using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.Core.Services
{
    public class ToDoItemService : IHandle<ToDoItemCompletedEvent>
    {
        public void Handle(ToDoItemCompletedEvent domainEvent)
        {
            // Do Nothing
        }
    }
}
