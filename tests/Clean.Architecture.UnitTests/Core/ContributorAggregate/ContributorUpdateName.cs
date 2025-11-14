using Clean.Architecture.Core.ContributorAggregate.Events;

namespace Clean.Architecture.UnitTests.Core.ContributorAggregate;

public class ContributorUpdateName
{
  private readonly ContributorName _initialName = ContributorName.From("initial name");
  private readonly ContributorName _newName = ContributorName.From("new name");
  private Contributor? _testContributor;
  private Contributor CreateContributor()
  {
    return new Contributor(_initialName);
  }

  [Fact]
  public void UpdatesName()
  {
    _testContributor = CreateContributor();
    _testContributor.UpdateName(_newName);
    _testContributor.Name.ShouldBe(_newName);
  }

  [Fact]
  public void RegistersDomainEvent()
  {
    _testContributor = CreateContributor();
    _testContributor.UpdateName(_newName);
    _testContributor.DomainEvents.Count.ShouldBe(1);
    _testContributor.DomainEvents.First().ShouldBeOfType<ContributorNameUpdatedEvent>();
  }

  [Fact]
  public void DoesNotRegisterDomainEventGivenCurrentName()
  {
    _testContributor = CreateContributor();
    _testContributor.UpdateName(_initialName);
    _testContributor.DomainEvents.Count.ShouldBe(0);
  }

}
