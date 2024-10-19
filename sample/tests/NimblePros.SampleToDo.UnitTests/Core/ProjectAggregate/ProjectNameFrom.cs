using FluentAssertions;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using Vogen;
using Xunit;

namespace NimblePros.SampleToDo.UnitTests.Core.ProjectAggregate;

public class ProjectNameFrom
{
  [Theory]
  [InlineData("")]
  [InlineData(null!)]
  public void ThrowsGivenNullOrEmpty(string name)
  {
    Assert.Throws<ValueObjectValidationException>(() => ProjectName.From(name));
  }

  [Fact]
  public void DoesNotThrowGivenValidData()
  {
    string validName = "valid name";
    var name = ProjectName.From(validName);
    name.Should().NotBeNull();
    name.Value.Should().Be(validName);
  }
}
