using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UnitTests.Core.ContributorAggregate;

public class ContributorConstructor
{
  private readonly string _testName = "test name";
  private Contributor? _testContributor;

  private Contributor CreateContributor()
  {
    return new Contributor(ContributorName.From(_testName));
  }

  [Fact]
  public void InitializesName()
  {
    _testContributor = CreateContributor();

    Assert.Equal(_testName, _testContributor.Name.Value);
  }
}

public class ContributorUpdateName
{
  private readonly string _testName = "new name";
  private Contributor? _testContributor;

  private Contributor CreateContributor()
  {
    return new Contributor(ContributorName.From(_testName));
  }

  [Fact]
  public void DoesNothingGivenSameName()
  {
    _testContributor = CreateContributor();
    var initialEvents = _testContributor.DomainEvents.Count;

    var initialHash = _testContributor.GetHashCode();

    _testContributor.UpdateName(ContributorName.From(_testName));

    Assert.Equal(initialHash, _testContributor.GetHashCode());
    Assert.Equal(initialEvents, _testContributor.DomainEvents.Count);
  }

  [Fact]
  public void UpdatesNameAndRegistersEventGivenNewName()
  {
    _testContributor = CreateContributor();
    var initialEvents = _testContributor.DomainEvents.Count;
    string newName = "A whole new name";

    _testContributor.UpdateName(ContributorName.From(newName));

    Assert.Equal(newName, _testContributor.Name.Value);
    Assert.Equal(1, _testContributor.DomainEvents.Count);
  }
}
