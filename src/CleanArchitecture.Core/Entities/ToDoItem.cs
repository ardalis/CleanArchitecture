using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class Bar : BaseEntity
    {
        public int Number { get; set; }
    }

    public class Foo : BaseEntity
    {
        public Bar Bar { get; set; }
        public string Name { get; set; }
    }

    public class ToDoItem : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; private set; }

        public void MarkComplete()
        {
            IsDone = true;
            Events.Add(new ToDoItemCompletedEvent(this));
        }
    }
}
