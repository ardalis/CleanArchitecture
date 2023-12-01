using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.UseCases.Contributors.Create;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Clean.Architecture.UnitTests.UseCases.Contributors;

public class CreateContributorHandlerHandle
{
  readonly string _testName = "test name";
  readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
  readonly CreateContributorHandler _handler;

  public CreateContributorHandlerHandle()
  {
    _handler = new CreateContributorHandler(_repository);
  }

  Contributor CreateContributor()
  {
    return new Contributor(_testName);
  }

  [Fact]
  public async Task ReturnsSuccessGivenValidName()
  {
    _repository.AddAsync(Arg.Any<Contributor>(), Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(CreateContributor()));
    Ardalis.Result.Result<int> result = await _handler.Handle(new CreateContributorCommand(_testName), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
  }
}
