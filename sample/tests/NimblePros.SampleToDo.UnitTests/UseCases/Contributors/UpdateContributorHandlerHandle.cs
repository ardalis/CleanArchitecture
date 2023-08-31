using Ardalis.Result;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using FluentAssertions;
using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ContributorAggregate.Specifications;
using NimblePros.SampleToDo.UseCases.Contributors.Update;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace NimblePros.SampleToDo.UnitTests.UseCases.Contributors;

public class UpdateContributorHandlerHandle
{
  private readonly string _testName = "test name";
  private readonly string _newName = Guid.NewGuid().ToString();
  private readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
  private UpdateContributorHandler _handler;

  public UpdateContributorHandlerHandle()
  {
      _handler = new UpdateContributorHandler(_repository);
  }

  [Fact]
  public async Task ReturnsRecordGivenValidId()
  {
    int validId = 1;
    _repository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
      .Returns(new Contributor(_testName));
    var result = await _handler.Handle(new UpdateContributorCommand(validId, _newName), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Name.Should().Be(_newName);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenInvalidId()
  {
    int invalidId = 1000;
    _repository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsNull();
    var result = await _handler.Handle(new UpdateContributorCommand(invalidId, _newName), CancellationToken.None);

    result.IsSuccess.Should().BeFalse();
    result.Status.Should().Be(ResultStatus.NotFound);
  }
}
