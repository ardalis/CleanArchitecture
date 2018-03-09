using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class ToDoItem : BaseEntity<ToDoItemKey>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; private set; } = false;

        public void MarkComplete()
        {
            IsDone = true;
            Events.Add(new ToDoItemCompletedEvent(this));
        }
    }

    public struct ToDoItemKey
    {
        private int _id;

        public ToDoItemKey(int id)
        {
            _id = id;
        }

        public static implicit operator ToDoItemKey(int value)
        {
            return new ToDoItemKey(value);
        }

        public static implicit operator int(ToDoItemKey me)
        {
            return me._id;
        }
    }
}