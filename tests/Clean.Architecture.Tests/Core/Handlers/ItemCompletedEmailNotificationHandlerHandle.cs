using System;
using System.Threading.Tasks;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.Core.Events;
using Clean.Architecture.Core.Services;
using Xunit;

namespace Clean.Architecture.UnitTests.Core.Entities
{
    public class ItemCompletedEmailNotificationHandlerHandle
    {
        [Fact]
        public async Task ThrowsExceptionGivenNullEventArgument()
        {
            var handler = new ItemCompletedEmailNotificationHandler();

            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null));
        }

        [Fact]
        public async Task DoesNothingGivenEventInstance()
        {
            var handler = new ItemCompletedEmailNotificationHandler();

            await handler.Handle(new ToDoItemCompletedEvent(new ToDoItem()));
        }
    }
}
