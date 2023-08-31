using FastEndpoints;
using MediatR;
using NimblePros.SampleToDo.UseCases.Projects.Create;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// Creates a new Project
/// </summary>
/// <remarks>
/// Creates a new project given a name.
/// </remarks>
public class Create : Endpoint<CreateProjectRequest, CreateProjectResponse>
{
  private readonly IMediator _mediator;

  public Create(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Post(CreateProjectRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new CreateProjectRequest { Name = "Project Name" };
    });
  }

  public override async Task HandleAsync(
  CreateProjectRequest request,
  CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new CreateProjectCommand(request.Name!));

    if (result.IsSuccess)
    {
      Response = new CreateProjectResponse(result.Value, request.Name!);
      return;
    }
    // TODO: Handle other cases as necessary
  }
}
