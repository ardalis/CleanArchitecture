using Ardalis.Result;
using FastEndpoints;
using MediatR;
using NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;
using NimblePros.SampleToDo.Web.Endpoints.ProjectEndpoints;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// Lists all incomplete items in a project.
/// </summary>
/// <remarks>
/// Lists all incomplete items in a project.
/// Returns FAKE data in DEV. Run in production to use real database-driven data.
/// </remarks>
public class ListIncompleteItems : Endpoint<ListIncompleteItemsRequest, ListIncompleteItemsResponse>
{
  private readonly IMediator _mediator;

  public ListIncompleteItems(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Get(ListIncompleteItemsRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(ListIncompleteItemsRequest request, CancellationToken cancellationToken)
  {
    Response = new ListIncompleteItemsResponse(request.ProjectId, new List<ToDoItemRecord>());

    var result = await _mediator.Send(new ListIncompleteItemsByProjectQuery(request.ProjectId));

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    Response.IncompleteItems = result.Value.Select(item =>
                    new ToDoItemRecord(item.Id, item.Title, item.Description, item.IsComplete, item.ContributorId))
                    .ToList();

  }
}
