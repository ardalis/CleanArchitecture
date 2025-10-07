using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.GetWithAllItems;
using NimblePros.SampleToDo.Web.Endpoints.Projects;
using NimblePros.SampleToDo.Web.Extensions;

namespace NimblePros.SampleToDo.Web.Projects;

public class GetById(IMediator mediator)
  : Endpoint<GetProjectByIdRequest,
             Results<Ok<GetProjectByIdResponse>,
                     NotFound,
                     ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetProjectByIdRequest.Route);
    AllowAnonymous();
    
    Summary(s =>
    {
      s.Summary = "Get a project by ID";
      s.Description = "Retrieves a specific project by its unique identifier. Returns detailed project information including all associated todo items.";
      s.ExampleRequest = new GetProjectByIdRequest { ProjectId = 1 };
      s.ResponseExamples[200] = new GetProjectByIdResponse(
        1, 
        "Sample Project", 
        new List<ToDoItemRecord> 
        { 
          new(1, "Sample Task", "Sample task description", false, 1) 
        });
      
      // Document possible responses
      s.Responses[200] = "Project found and returned successfully";
      s.Responses[404] = "Project with specified ID not found";
      s.Responses[400] = "Invalid project ID provided";
    });
    
    // Add tags for API grouping
    Tags("Projects");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<GetProjectByIdRequest>()
      .Produces<GetProjectByIdResponse>(200, "application/json")
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<Ok<GetProjectByIdResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetProjectByIdRequest request, CancellationToken ct)
  {
    var command = new GetProjectWithAllItemsQuery(ProjectId.From(request.ProjectId));
    var result = await mediator.Send(command, ct);

    return result.ToGetByIdResult(project => new GetProjectByIdResponse(
      project.Id.Value,
      project.Name.Value,
      project.Items
        .Select(item => new ToDoItemRecord(
          item.Id.Value,
          item.Title,
          item.Description,
          item.IsComplete,
          item.ContributorId?.Value))
        .ToList()));
  }
}
