using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects.Create;
using NimblePros.SampleToDo.Web.Extensions;

namespace NimblePros.SampleToDo.Web.Projects;

public class Create(IMediator mediator) 
  : Endpoint<CreateProjectRequest, Results<Created<CreateProjectResponse>, ValidationProblem, ProblemHttpResult>>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post(CreateProjectRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Create a new project";
      s.Description = "Creates a new project with the provided name. The project name must be between 2 and 100 characters long.";
      s.ExampleRequest = new CreateProjectRequest { Name = "My New Project" };
      s.ResponseExamples[201] = new CreateProjectResponse(1, "My New Project");
      
      // Document possible responses
      s.Responses[201] = "Project created successfully";
      s.Responses[400] = "Invalid input data - validation errors";
      s.Responses[500] = "Internal server error";
    });
    
    // Add tags for API grouping
    Tags("Projects");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<CreateProjectRequest>("application/json")
      .Produces<CreateProjectResponse>(201, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(500));
  }

  public override async Task<Results<Created<CreateProjectResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateProjectRequest request, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new CreateProjectCommand(ProjectName.From(request.Name!)));

    return result.ToCreatedResult(
      id => $"/Projects/{id}", 
      id => new CreateProjectResponse(id.Value, request.Name!));
  }
}
