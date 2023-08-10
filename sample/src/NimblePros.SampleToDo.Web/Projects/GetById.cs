using Ardalis.Result;
using FastEndpoints;
using MediatR;
using NimblePros.SampleToDo.UseCases.Projects.GetWithAllItems;
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
    Get(GetProjectByIdRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetProjectByIdRequest request,
  CancellationToken cancellationToken)
  {
    var command = new GetProjectWithAllItemsQuery(request.ProjectId);

    var result = await _mediator.Send(command);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new GetProjectByIdResponse(result.Value.Id,
        result.Value.Name, 
        items:
        result.Value.Items
          .Select(item => new ToDoItemRecord(
            item.Id,
            item.Title,
            item.Description,
            item.IsComplete,
            item.ContributorId
            ))
          .ToList()
          );
    }
  }
}
