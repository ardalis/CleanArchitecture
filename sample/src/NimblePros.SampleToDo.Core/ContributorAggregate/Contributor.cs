namespace NimblePros.SampleToDo.Core.ContributorAggregate;

public class Contributor : EntityBase, IAggregateRoot
{
  public string Name { get; private set; } = default!;

  public Contributor(string name)
  {
    SetName(name);
  }

  public void UpdateName(string newName)
  {
    SetName(newName);
  }

  public Contributor SetName(string newName)
  {
    this.Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
    return this;
  }
}
