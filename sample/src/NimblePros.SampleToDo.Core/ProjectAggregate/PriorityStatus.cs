namespace NimblePros.SampleToDo.Core.ProjectAggregate;

public class PriorityStatus : SmartEnum<PriorityStatus>
{
  public static readonly PriorityStatus Backlog = new(nameof(Backlog), 0);
  public static readonly PriorityStatus Critical = new(nameof(Critical), 1);

  protected PriorityStatus(string name, int value) : base(name, value) { }
}

