using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ContributorAggregate.Specifications;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Update;
using Shouldly;

namespace NimblePros.SampleToDo.UnitTests.UseCases.Contributors;

public class UpdateContributorHandlerHandle
{
  private readonly ContributorName _testName = ContributorName.From("test name");
  private readonly ContributorName _newName = ContributorName.From(Guid.NewGuid().ToString());
  private readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
  private UpdateContributorHandler _handler;

  public UpdateContributorHandlerHandle()
  {
      _handler = new UpdateContributorHandler(_repository);
  }

  [Fact]
  public async Task ReturnsRecordGivenValidId()
  {
    ContributorId validId = ContributorId.From(1);
    _repository.SingleOrDefaultAsync(Arg.Any<ContributorByIdSpec>(), Arg.Any<CancellationToken>())
      .Returns(new Contributor(_testName) { Id = ContributorId.From(1)});
    var result = await _handler.Handle(new UpdateContributorCommand(validId, _newName), CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Value.Name.ShouldBe(_newName);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenNonexistentId()
  {
    ContributorId nonexistentId = ContributorId.From(1000);
    _repository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsNull();
    var result = await _handler.Handle(new UpdateContributorCommand(nonexistentId, _newName), CancellationToken.None);

    result.IsSuccess.ShouldBeFalse();
    result.Status.ShouldBe(ResultStatus.NotFound);
  }
}
