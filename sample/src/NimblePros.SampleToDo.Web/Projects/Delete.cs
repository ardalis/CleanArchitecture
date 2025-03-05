using Ardalis.Result.AspNetCore;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.Delete;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// Deletes a project
/// </summary>
public class Delete(IMediator mediator) : Endpoint<DeleteProjectRequest>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Delete(DeleteProjectRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
  DeleteProjectRequest request,
  CancellationToken cancellationToken)
  {
    var command = new DeleteProjectCommand(ProjectId.From(request.ProjectId));

    var result = await _mediator.Send(command);

    await SendResultAsync(result.ToMinimalApiResult());
  }
}
