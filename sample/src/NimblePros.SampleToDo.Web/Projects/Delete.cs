using Ardalis.Result;
using FastEndpoints;
using MediatR;
using NimblePros.SampleToDo.UseCases.Projects.Delete;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// Deletes a project
/// </summary>
public class Delete : Endpoint<DeleteProjectRequest>
{
  private readonly IMediator _mediator;

  public Delete(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Delete(DeleteProjectRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
  DeleteProjectRequest request,
  CancellationToken cancellationToken)
  {
    var command = new DeleteProjectCommand(request.ProjectId);

    var result = await _mediator.Send(command);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await SendNoContentAsync(cancellationToken);
    };
    // TODO: Handle other issues as needed
  }
}
