using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.Services;
using Ardalis.SharedKernel;
using MediatR;
using Xunit;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace NimblePros.SampleToDo.UnitTests.Core.Services;

public class DeleteContributorService_DeleteContributor
{
  private readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
  private readonly IMediator _mediator = Substitute.For<IMediator>();
  private readonly ILogger<DeleteContributorService> _logger = Substitute.For<ILogger<DeleteContributorService>>();

  private readonly DeleteContributorService _service;

  public DeleteContributorService_DeleteContributor()
  {
    _service = new DeleteContributorService(_repository, _mediator, _logger);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenCantFindContributor()
  {
    var result = await _service.DeleteContributor(0);

    Assert.Equal(Ardalis.Result.ResultStatus.NotFound, result.Status);
  }
}
