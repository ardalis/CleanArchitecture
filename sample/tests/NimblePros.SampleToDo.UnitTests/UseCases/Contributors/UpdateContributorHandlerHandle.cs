using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Update;
using Shouldly;

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
      .Returns(new Contributor(ContributorName.From(_testName)));
    var result = await _handler.Handle(new UpdateContributorCommand(validId, ContributorName.From(_newName)), CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Value.Name.ShouldBe(_newName);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenInvalidId()
  {
    int invalidId = 1000;
    _repository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsNull();
    var result = await _handler.Handle(new UpdateContributorCommand(invalidId, ContributorName.From(_newName)), CancellationToken.None);

    result.IsSuccess.ShouldBeFalse();
    result.Status.ShouldBe(ResultStatus.NotFound);
  }
}
