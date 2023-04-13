using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class Update : EndpointBaseAsync
    .WithRequest<UpdateProjectRequest>
    .WithActionResult<UpdateProjectResponse>
{
  private readonly IRepository<Project> _repository;

  public Update(IRepository<Project> repository)
  {
    _repository = repository;
  }

  [HttpPut(UpdateProjectRequest.Route)]
  [SwaggerOperation(
      Summary = "Updates a Project",
      Description = "Updates a Project. Only supports changing the name.",
      OperationId = "Projects.Update",
      Tags = new[] { "ProjectEndpoints" })
  ]
  public override async Task<ActionResult<UpdateProjectResponse>> HandleAsync(
    UpdateProjectRequest request,
      CancellationToken cancellationToken = new ())
  {
    if (request.Name == null)
    {
      return BadRequest();
    }

    var existingProject = await _repository.GetByIdAsync(request.Id, cancellationToken);
    if (existingProject == null)
    {
      return NotFound();
    }

    existingProject.UpdateName(request.Name);

    await _repository.UpdateAsync(existingProject, cancellationToken);

    var response = new UpdateProjectResponse(
        project: new ProjectRecord(existingProject.Id, existingProject.Name)
    );

    return Ok(response);
  }
}
