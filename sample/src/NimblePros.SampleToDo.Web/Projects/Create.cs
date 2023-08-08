using Ardalis.ApiEndpoints;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;
using NimblePros.SampleToDo.Web.Endpoints.ProjectEndpoints;

namespace NimblePros.SampleToDo.Web.ProjectEndpoints;

public class Create : EndpointBaseAsync
  .WithRequest<CreateProjectRequest>
  .WithActionResult<CreateProjectResponse>
{
  private readonly IRepository<Project> _repository;

  public Create(IRepository<Project> repository)
  {
    _repository = repository;
  }

  [HttpPost("/Projects")]
  //[SwaggerOperation(
  //  Summary = "Creates a new Project",
  //  Description = "Creates a new Project",
  //  OperationId = "Project.Create",
  //  Tags = new[] { "ProjectEndpoints" })
  //]
  public override async Task<ActionResult<CreateProjectResponse>> HandleAsync(
    CreateProjectRequest request,
    CancellationToken cancellationToken = new())
  {
    if (request.Name == null)
    {
      return BadRequest();
    }

    var newProject = new Project(request.Name, PriorityStatus.Backlog);
    var createdItem = await _repository.AddAsync(newProject, cancellationToken);
    var response = new CreateProjectResponse
    (
      id: createdItem.Id,
      name: createdItem.Name
    );

    return Ok(response);
  }
}
