namespace Clean.Architecture.UnitTests.Core.ContributorAggregate;

public class ContributorIdFrom
{
  [Fact]
  public void CreatesGivenValidValue()
  {
    int validValue = 1;
    var contributorId = ContributorId.From(validValue);
    Assert.Equal(validValue, contributorId.Value);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public void ThrowsGivenInvalidValue(int invalidValue)
  {
    Assert.Throws<Vogen.ValueObjectValidationException>(() => ContributorId.From(invalidValue));
  }
}
