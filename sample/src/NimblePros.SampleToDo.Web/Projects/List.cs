using FastEndpoints;
using MediatR;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.ListShallow;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// Lists all projects without their sub-properties.
/// </summary>
/// <remarks>
/// Lists all projects without their sub-properties.
/// NOTE: In DEV will always show a FAKE ID 1000 project, not real data
/// </remarks>
public class List : EndpointWithoutRequest<ProjectListResponse>
{
  private readonly IMediator _mediator;

  public List(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Get($"/{nameof(Project)}s");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new ListProjectsShallowQuery(null, null));

    if (result.IsSuccess)
    {
      Response = new ProjectListResponse
      {
        Projects = result.Value.Select(c => new ProjectRecord(c.Id, c.Name)).ToList()
      };
    }
  }
}
