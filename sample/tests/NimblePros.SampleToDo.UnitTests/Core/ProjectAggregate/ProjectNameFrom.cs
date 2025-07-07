using Shouldly;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using Vogen;

namespace NimblePros.SampleToDo.UnitTests.Core.ProjectAggregate;

public class ProjectNameFrom
{
  [Theory]
  [InlineData("")]
  [InlineData(null!)]
  public void ThrowsGivenNullOrEmpty(string name)
  {
    Should.Throw<ValueObjectValidationException>(() => ProjectName.From(name));
  }

  [Fact]
  public void DoesNotThrowGivenValidData()
  {
    string validName = "valid name";
    var name = ProjectName.From(validName);
    name.Value.ShouldBe(validName);
  }
}
