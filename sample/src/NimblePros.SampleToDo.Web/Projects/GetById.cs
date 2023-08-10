using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NimblePros.SampleToDo.Core.ProjectAggregate.Specifications;
using NimblePros.SampleToDo.UseCases.Contributors.Get;
using NimblePros.SampleToDo.Web.Contributors;
using NimblePros.SampleToDo.Web.Endpoints.Projects;

namespace NimblePros.SampleToDo.Web.Projects;

public class GetById : Endpoint<GetProjectByIdRequest, GetProjectByIdResponse>
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

  public override async Task HandleAsync(GetProjectByIdRequest request,
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
      Response = new GetProjectByIdResponse(result.Value.Id, result.Value.Name);
    }
  }



  public override async Task<ActionResult<GetProjectByIdResponse>> HandleAsync(
    [FromRoute] GetProjectByIdRequest request,
    CancellationToken cancellationToken = new())
  {
    var spec = new ProjectByIdWithItemsSpec(request.ProjectId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null)
    {
      return NotFound();
    }

    var response = new GetProjectByIdResponse
    (
      id: entity.Id,
      name: entity.Name,
      items: entity.Items.Select(
        item => new ToDoItemRecord(item.Id,
          item.Title,
          item.Description,
          item.IsDone,
          item.ContributorId))
        .ToList()
    );

    return Ok(response);
  }
}
