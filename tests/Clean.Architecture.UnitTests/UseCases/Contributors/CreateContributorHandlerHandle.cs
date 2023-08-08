using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.UseCases.Contributors.Create;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clean.Architecture.UnitTests.UseCases.Contributors;

public class CreateContributorHandlerHandle
{
  private readonly string _testName = "test name";
  //private Contributor? _testContributor;
  private Mock<IRepository<Contributor>> _mockRepository = new Mock<IRepository<Contributor>>();
  private CreateContributorHandler _handler;

  public CreateContributorHandlerHandle()
  {
      _handler = new CreateContributorHandler(_mockRepository.Object);
  }

  private Contributor CreateContributor()
  {
    return new Contributor(_testName);
  }

  [Fact]
  public async Task ReturnsSuccessGivenValidName()
  {
    _mockRepository.Setup(r => r.AddAsync(It.IsAny<Contributor>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(CreateContributor());
    var result = await _handler.Handle(new CreateContributorCommand(_testName), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();    
  }
}
