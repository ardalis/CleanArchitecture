using FastEndpoints;
using MediatR;
using NimblePros.SampleToDo.UseCases.Projects.AddToDoItem;
using NimblePros.SampleToDo.Web.Projects;

namespace NimblePros.SampleToDo.Web.ProjectEndpoints;

public class Create : Endpoint<CreateToDoItemRequest>
{
  private readonly IMediator _mediator;

  public Create(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Post(CreateToDoItemRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new CreateToDoItemRequest
      {
        ContributorId = 1,
        ProjectId = 1,
        Title = "Title",
        Description = "Description"
      };
    });
  }

  public override async Task HandleAsync(
    CreateToDoItemRequest request,
    CancellationToken cancellationToken)
  {
    var command = new AddToDoItemCommand(request.ProjectId, request.ContributorId,
      request.Title, request.Description);
    var result = await _mediator.Send(command);

    if (result.Status == Ardalis.Result.ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      // send route to project
      await SendCreatedAtAsync<GetById>(new { projectId = request.ProjectId }, "");
    };
    // TODO: Handle other cases as necessary
  }
}
