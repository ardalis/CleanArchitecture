using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Tests
{
    public class ToDoItemBuilder
    {
        private readonly ToDoItem _todo = new ToDoItem();

        public ToDoItemBuilder Id(int id)
        {
            _todo.Id = id;
            return this;
        }

        public ToDoItemBuilder Title(string title)
        {
            _todo.Title = title;
            return this;
        }

        public ToDoItemBuilder Description(string description)
        {
            _todo.Description = description;
            return this;
        }

        public ToDoItem Build() => _todo;
    }
}
