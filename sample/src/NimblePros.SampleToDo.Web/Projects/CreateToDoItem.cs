using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.AddToDoItem;
using NimblePros.SampleToDo.Web.Extensions;
using NimblePros.SampleToDo.Web.Projects;

namespace NimblePros.SampleToDo.Web.ProjectEndpoints;

public class Create : Endpoint<CreateToDoItemRequest, Results<Created, NotFound, ProblemHttpResult>>
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
      s.Summary = "Add a new todo item to a project";
      s.Description = "Creates a new todo item within an existing project. The project must exist and the contributor (if specified) must be valid.";
      s.ExampleRequest = new CreateToDoItemRequest
      {
        ContributorId = 1,
        ProjectId = 1,
        Title = "Implement user authentication",
        Description = "Add JWT-based authentication to the API"
      };
      
      // Document possible responses
      s.Responses[201] = "Todo item created successfully";
      s.Responses[404] = "Project or contributor not found";
      s.Responses[400] = "Invalid input data";
      s.Responses[500] = "Internal server error";
    });
    
    // Add tags for API grouping
    Tags("Projects");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<CreateToDoItemRequest>("application/json")
      .Produces(201)
      .ProducesProblem(404)
      .ProducesProblem(400)
      .ProducesProblem(500));
  }

  public override async Task<Results<Created, NotFound, ProblemHttpResult>>
    ExecuteAsync(CreateToDoItemRequest request, CancellationToken cancellationToken)
  {
    ContributorId? contributorId = request.ContributorId.HasValue
                                    ? ContributorId.From(request.ContributorId.Value)
                                    : null;
    var command = new AddToDoItemCommand(ProjectId.From(request.ProjectId), contributorId,
      request.Title, request.Description);
    var result = await _mediator.Send(command);

    return result.Status switch
    {
      ResultStatus.Ok => TypedResults.Created($"/Projects/{request.ProjectId}"),
      ResultStatus.NotFound => TypedResults.NotFound(),
      _ => TypedResults.Problem(
        title: "Create todo item failed",
        detail: string.Join("; ", result.Errors),
        statusCode: StatusCodes.Status400BadRequest)
    };
  }
}
