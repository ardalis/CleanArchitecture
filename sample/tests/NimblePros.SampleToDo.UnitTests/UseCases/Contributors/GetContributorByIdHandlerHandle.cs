using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ContributorAggregate.Specifications;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.GetById;
using Shouldly;

namespace NimblePros.SampleToDo.UnitTests.UseCases.Contributors;

public class GetContributorByIdHandlerHandle
{
  private readonly ContributorName _testName = ContributorName.From("test name");
  private readonly IReadRepository<Contributor> _repository = Substitute.For<IReadRepository<Contributor>>();
  private GetContributorByIdHandler _handler;

  public GetContributorByIdHandlerHandle()
  {
      _handler = new GetContributorByIdHandler(_repository);
  }

  [Fact]
  public async Task ReturnsRecordGivenValidId()
  {
    ContributorId validId = ContributorId.From(1);
    _repository.FirstOrDefaultAsync(Arg.Any<ContributorByIdSpec>(), Arg.Any<CancellationToken>())
      .Returns(new Contributor(_testName));
    var result = await _handler.Handle(new GetContributorByIdQuery(validId), CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Value.Name.ShouldBe(_testName);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenInvalidId()
  {
    ContributorId invalidId = ContributorId.From(1000);
    _repository.FirstOrDefaultAsync(Arg.Any<ContributorByIdSpec>(), Arg.Any<CancellationToken>()).ReturnsNull();
    var result = await _handler.Handle(new GetContributorByIdQuery(invalidId), CancellationToken.None);

    result.IsSuccess.ShouldBeFalse();
    result.Status.ShouldBe(ResultStatus.NotFound);
  }
}
