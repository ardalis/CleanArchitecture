using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.MarkToDoItemComplete;

namespace NimblePros.SampleToDo.Web.ProjectEndpoints;

/// <summary>
/// Mark an item as complete
/// </summary>
public class MarkItemComplete(IMediator mediator) : Endpoint<MarkItemCompleteRequest>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post(MarkItemCompleteRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new MarkItemCompleteRequest
      {
        ProjectId = 1,
        ToDoItemId = 1
      };
    });
  }

  public override async Task HandleAsync(
    MarkItemCompleteRequest request,
    CancellationToken cancellationToken)
  {
    var command = new MarkToDoItemCompleteCommand(ProjectId.From(request.ProjectId), request.ToDoItemId);
    var result = await _mediator.Send(command);

    if (result.Status == Ardalis.Result.ResultStatus.NotFound)
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
