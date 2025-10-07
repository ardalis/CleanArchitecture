using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.Delete;
using NimblePros.SampleToDo.Web.Extensions;

namespace NimblePros.SampleToDo.Web.Projects;

public class Delete
  : Endpoint<DeleteProjectRequest,
             Results<NoContent,
                     NotFound,
                     ProblemHttpResult>>
{
  private readonly IMediator _mediator;
  public Delete(IMediator mediator) => _mediator = mediator;

  public override void Configure()
  {
    Delete(DeleteProjectRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Delete a project";
      s.Description = "Deletes an existing project by ID. This will also delete all associated todo items. This action cannot be undone.";
      s.ExampleRequest = new DeleteProjectRequest { ProjectId = 1 };
      
      // Document possible responses
      s.Responses[204] = "Project deleted successfully";
      s.Responses[404] = "Project not found";
      s.Responses[400] = "Invalid request or deletion failed";
    });
    
    // Add tags for API grouping
    Tags("Projects");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<DeleteProjectRequest>()
      .Produces(204)
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeleteProjectRequest req, CancellationToken ct)
  {
    var cmd = new DeleteProjectCommand(ProjectId.From(req.ProjectId));
    var result = await _mediator.Send(cmd, ct);

    return result.ToDeleteResult();
  }
}
