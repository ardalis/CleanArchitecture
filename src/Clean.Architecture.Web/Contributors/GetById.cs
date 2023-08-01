using FastEndpoints;
using MediatR;
using Ardalis.Result;
using Clean.Architecture.Web.Endpoints.ContributorEndpoints;
using Clean.Architecture.UseCases.Contributors.Get;

namespace Clean.Architecture.Web.ContributorEndpoints;

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

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    Response = new ContributorRecord(result.Value.Id, result.Value.Name);
  }
}
