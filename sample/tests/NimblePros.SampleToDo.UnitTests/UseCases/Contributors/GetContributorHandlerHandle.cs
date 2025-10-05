using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ContributorAggregate.Specifications;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.Get;
using Shouldly;

namespace NimblePros.SampleToDo.UnitTests.UseCases.Contributors;

public class GetContributorHandlerHandle
{
  private readonly ContributorName _testName = ContributorName.From("test name");
  private readonly IReadRepository<Contributor> _repository = Substitute.For<IReadRepository<Contributor>>();
  private GetContributorHandler _handler;

  public GetContributorHandlerHandle()
  {
      _handler = new GetContributorHandler(_repository);
  }

  [Fact]
  public async Task ReturnsRecordGivenValidId()
  {
    ContributorId validId = ContributorId.From(1);
    _repository.FirstOrDefaultAsync(Arg.Any<ContributorByIdSpec>(), Arg.Any<CancellationToken>())
      .Returns(new Contributor(_testName));
    var result = await _handler.Handle(new GetContributorQuery(validId), CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Value.Name.ShouldBe(_testName);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenInvalidId()
  {
    ContributorId invalidId = ContributorId.From(1000);
    _repository.FirstOrDefaultAsync(Arg.Any<ContributorByIdSpec>(), Arg.Any<CancellationToken>()).ReturnsNull();
    var result = await _handler.Handle(new GetContributorQuery(invalidId), CancellationToken.None);

    result.IsSuccess.ShouldBeFalse();
    result.Status.ShouldBe(ResultStatus.NotFound);
  }
}
