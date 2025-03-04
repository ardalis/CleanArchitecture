using Ardalis.Result.AspNetCore;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.Update;

namespace NimblePros.SampleToDo.Web.Projects;

public class Update(IMediator mediator) : Endpoint<UpdateProjectRequest, UpdateProjectResponse>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Put(UpdateProjectRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
  UpdateProjectRequest request,
  CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new UpdateProjectCommand(ProjectId.From(request.Id), ProjectName.From(request.Name!)));

    await SendResultAsync(result.ToMinimalApiResult());
  }
}
