using CleanArchitecture.Core.Model;
using CleanArchitecture.Core.Events;
using System.Linq;
using Xunit;

namespace CleanArchitecture.Tests.Core.Model
{
    public class ToDoItemMarkCompleteShould
    {
        [Fact]
        public void SetIsDoneToTrue()
        {
            var item = new ToDoItem();

            item.MarkComplete();

            Assert.True(item.IsDone);
        }

        [Fact]
        public void RaiseToDoItemCompletedEvent()
        {
            var item = new ToDoItem();

            item.MarkComplete();

            Assert.Equal(1, item.Events.Count());
            Assert.IsType<ToDoItemCompletedEvent>(item.Events.First());
        }
    }
}