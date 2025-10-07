using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects;
using NimblePros.SampleToDo.UseCases.Projects.Update;
using NimblePros.SampleToDo.Web.Extensions;

namespace NimblePros.SampleToDo.Web.Projects;

public class Update(IMediator mediator)
  : Endpoint<
        UpdateProjectRequest,
        Results<Ok<UpdateProjectResponse>, NotFound, ProblemHttpResult>,
        UpdateProjectMapper>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Put(UpdateProjectRequest.Route);
    AllowAnonymous();

    // Optional but nice: enumerate for Swagger
    Summary(s =>
    {
      s.Summary = "Update a project";
      s.Description = "Updates an existing project's information. The project name must be between 2 and 100 characters long.";
      s.ExampleRequest = new UpdateProjectRequest { Id = 1, Name = "Updated Project Name" };
      s.ResponseExamples[200] = new UpdateProjectResponse(new ProjectRecord(1, "Updated Project Name"));
      
      // Document possible responses
      s.Responses[200] = "Project updated successfully";
      s.Responses[404] = "Project with specified ID not found";
      s.Responses[400] = "Invalid input data or business rule violation";
    });
    
    // Add tags for API grouping
    Tags("Projects");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<UpdateProjectRequest>("application/json")
      .Produces<UpdateProjectResponse>(200, "application/json")
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<Ok<UpdateProjectResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdateProjectRequest request, CancellationToken ct)
  {
    var cmd = new UpdateProjectCommand(
      ProjectId.From(request.Id),
      ProjectName.From(request.Name!));

    var result = await _mediator.Send(cmd, ct);

    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class UpdateProjectMapper
  : Mapper<UpdateProjectRequest, UpdateProjectResponse, ProjectDto>
{
  public override UpdateProjectResponse FromEntity(ProjectDto e)
    => new(new ProjectRecord(e.Id, e.Name));
}
