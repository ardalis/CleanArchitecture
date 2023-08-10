using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.ListShallow;
using NimblePros.SampleToDo.Web.Endpoints.Projects;

namespace NimblePros.SampleToDo.Web.Projects;

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
