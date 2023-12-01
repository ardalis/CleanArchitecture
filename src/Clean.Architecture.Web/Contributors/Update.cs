using Ardalis.Result;
using Clean.Architecture.UseCases.Contributors.Get;
using Clean.Architecture.UseCases.Contributors.Update;
using FastEndpoints;
using MediatR;

namespace Clean.Architecture.Web.Contributors;

/// <summary>
/// Update an existing Contributor.
/// </summary>
/// <remarks>
/// Update an existing Contributor by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator _mediator)
  : Endpoint<UpdateContributorRequest, UpdateContributorResponse>
{
  public override void Configure()
  {
    Put(UpdateContributorRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
    UpdateContributorRequest request,
    CancellationToken cancellationToken)
  {
    Result<UseCases.Contributors.ContributorDTO> result = await _mediator.Send(new UpdateContributorCommand(request.Id, request.Name!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var query = new GetContributorQuery(request.ContributorId);

    Result<UseCases.Contributors.ContributorDTO> queryResult = await _mediator.Send(query, cancellationToken);

    if (queryResult.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (queryResult.IsSuccess)
    {
      UseCases.Contributors.ContributorDTO dto = queryResult.Value;
      Response = new UpdateContributorResponse(new ContributorRecord(dto.Id, dto.Name));
      return;
    }
  }
}
