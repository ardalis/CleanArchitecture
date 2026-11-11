namespace Clean.Architecture.UnitTests.Core.ContributorAggregate;

public class ContributorNameFrom
{
  [Fact]
  public void CreatesGivenValidValue()
  {
    string validValue = "ardalis";
    var contributorName = ContributorName.From(validValue);
    contributorName.Value.ShouldBe(validValue);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void ThrowsGivenInvalidValue(string? invalidValue)
  {
    Assert.Throws<Vogen.ValueObjectValidationException>(() => ContributorName.From(invalidValue!));
  }
}
