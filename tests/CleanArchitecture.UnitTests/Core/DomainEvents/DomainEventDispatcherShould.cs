using Autofac;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.DomainEvents;
using Xunit;

namespace CleanArchitecture.UnitTests.Core.DomainEvents
{
    public class DomainEventDispatcherShould
    {
        [Fact]
        public void NotReturnAnEmptyListOfAvailableHandlers()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DefaultInfrastructureModule(isDevelopment: true));
            var container = builder.Build();

            var domainEventDispatcher = new DomainEventDispatcher(container);
            var toDoItemCompletedEvent = new ToDoItemCompletedEvent(new ToDoItem());

            var handlersForEvent = domainEventDispatcher.GetWrappedHandlers(toDoItemCompletedEvent);

            Assert.NotEmpty(handlersForEvent);
        }
    }
}
