using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ContributorAggregate.Specifications;
using Ardalis.SharedKernel;
using FastEndpoints;
using Clean.Architecture.UseCases.Queries.GetContributor;
using MediatR;
using Ardalis.Result;

namespace Clean.Architecture.Web.Endpoints.ContributorEndpoints;

public class GetById : Endpoint<GetContributorByIdRequest, ContributorRecord>
{
  private readonly IMediator _mediator;

  public GetById(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Get(GetContributorByIdRequest.Route);
    AllowAnonymous();
    Options(x => x
      .WithTags("ContributorEndpoints"));
  }
  public override async Task HandleAsync(GetContributorByIdRequest request, 
    CancellationToken cancellationToken)
  {
    var command = new GetContributorCommand(request.ContributorId);

    var result = await _mediator.Send(command);

    if(result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    Response = new ContributorRecord(result.Value.Id, result.Value.Name);
  }
}
