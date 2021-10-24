using Clean.Architecture.Core.ProjectAggregate.Events;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ProjectAggregate;

public class ToDoItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public bool IsDone { get; private set; }

    public void MarkComplete()
    {
        if (!IsDone)
        {
            IsDone = true;

            Events.Add(new ToDoItemCompletedEvent(this));
        }
    }

    public override string ToString()
    {
        string status = IsDone ? "Done!" : "Not done.";
        return $"{Id}: Status: {status} - {Title} - {Description}";
    }
}
