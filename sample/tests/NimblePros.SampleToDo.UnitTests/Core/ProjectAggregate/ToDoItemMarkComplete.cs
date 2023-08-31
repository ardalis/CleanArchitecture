using NimblePros.SampleToDo.Core.ProjectAggregate.Events;
using Xunit;

namespace NimblePros.SampleToDo.UnitTests.Core.ProjectAggregate;

public class ToDoItemMarkComplete
{
  [Fact]
  public void SetsIsDoneToTrue()
  {
    var item = new ToDoItemBuilder()
        .WithDefaultValues()
        .Description("")
        .Build();

    item.MarkComplete();

    Assert.True(item.IsDone);
  }

  [Fact]
  public void RaisesToDoItemCompletedEvent()
  {
    var item = new ToDoItemBuilder().Build();

    item.MarkComplete();

    Assert.Single(item.DomainEvents);
    Assert.IsType<ToDoItemCompletedEvent>(item.DomainEvents.First());
  }
}
