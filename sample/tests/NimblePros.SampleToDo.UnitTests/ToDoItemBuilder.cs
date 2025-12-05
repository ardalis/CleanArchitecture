using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UnitTests;

// Learn more about test builders:
// https://ardalis.com/improve-tests-with-the-builder-pattern-for-test-data
public class ToDoItemBuilder
{
  private ToDoItem _todo = new ToDoItem();

  public ToDoItemBuilder Id(int id)
  {
    _todo.Id = ToDoItemId.From(id);
    return this;
  }

  public ToDoItemBuilder Title(String title)
  {
    _todo.UpdateTitle(ToDoItemTitle.From(title));
    return this;
  }

  public ToDoItemBuilder Description(String description)
  {
    _todo.UpdateDescription(ToDoItemDescription.From(description));
    return this;
  }

  public ToDoItemBuilder WithDefaultValues()
  {
    _todo = new ToDoItem(title: ToDoItemTitle.From("Test Item"), description: ToDoItemDescription.From("Test Description")) { Id = ToDoItemId.From(1) };

    return this;
  }

  public ToDoItem Build() => _todo;
}
