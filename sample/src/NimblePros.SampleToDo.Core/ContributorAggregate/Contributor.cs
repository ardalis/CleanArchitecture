namespace NimblePros.SampleToDo.Core.ContributorAggregate;

public class Contributor : EntityBase, IAggregateRoot
{
  public string Name { get; private set; } = default!;

  public Contributor(string name)
  {
    UpdateName(name);
  }

  public Contributor UpdateName(string newName)
  {
    this.Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
    return this;
  }
}
