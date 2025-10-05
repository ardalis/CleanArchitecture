using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Delete;

namespace NimblePros.SampleToDo.Web.Contributors;

/// <summary>
/// Delete a Contributor.
/// </summary>
/// <remarks>
/// Delete a Contributor by providing a valid integer id.
/// </remarks>
public class Delete : Endpoint<DeleteContributorRequest>
{
  private readonly IMediator _mediator;

  public Delete(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Delete(DeleteContributorRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
    DeleteContributorRequest request,
    CancellationToken cancellationToken)
  {
    var command = new DeleteContributorCommand(ContributorId.From(request.ContributorId));

    var result = await _mediator.Send(command);

    if (result.Status == ResultStatus.NotFound)
    {
      await Send.NotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await Send.NoContentAsync(cancellationToken);
    };
    // TODO: Handle other issues as needed
  }
}
