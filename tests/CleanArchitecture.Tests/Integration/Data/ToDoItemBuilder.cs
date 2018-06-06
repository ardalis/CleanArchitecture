using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Tests.Integration.Data
{
    public class ToDoItemBuilder
    {
        private readonly ToDoItem _todo = new ToDoItem();

        public ToDoItemBuilder Title(string title)
        {
            _todo.Title = title;
            return this;
        }

        public ToDoItem Build() => _todo;
    }
}
