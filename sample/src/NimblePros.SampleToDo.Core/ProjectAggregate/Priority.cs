using Ardalis.SmartEnum;

namespace NimblePros.SampleToDo.Core.ProjectAggregate;

public class Priority : SmartEnum<Priority>
{
  public static readonly Priority Backlog = new(nameof(Backlog), 0);
  public static readonly Priority Critical = new(nameof(Critical), 1);

  protected Priority(string name, int value) : base(name, value) { }
}
