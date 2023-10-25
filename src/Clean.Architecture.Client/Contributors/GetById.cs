using FastEndpoints;
using MediatR;
using Ardalis.Result;
using Clean.Architecture.Web.Endpoints.ContributorEndpoints;
using Clean.Architecture.UseCases.Contributors.Get;

namespace Clean.Architecture.Web.ContributorEndpoints;

/// <summary>
/// Get a Contributor by integer ID.
/// </summary>
/// <remarks>
/// Takes a positive integer ID and returns a matching Contributor record.
/// </remarks>
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
  }

  public override async Task HandleAsync(GetContributorByIdRequest request,
    CancellationToken cancellationToken)
  {
    var command = new GetContributorQuery(request.ContributorId);

    var result = await _mediator.Send(command);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new ContributorRecord(result.Value.Id, result.Value.Name);
    }
  }
}
