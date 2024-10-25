using NimblePros.SampleToDo.Core.ProjectAggregate;
using Xunit;

namespace NimblePros.SampleToDo.UnitTests.Core.ProjectAggregate;

public class ToDoItemConstructor
{
  [Fact]
  public void InitializesPriority()
  {
    var item = new ToDoItemBuilder()
    .WithDefaultValues()
    .Build();

    Assert.Equal(item.Priority, Priority.Backlog);
  }
}
