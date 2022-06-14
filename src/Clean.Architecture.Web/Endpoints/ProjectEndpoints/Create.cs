﻿using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

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
  [SwaggerOperation(
    Summary = "Creates a new Project",
    Description = "Creates a new Project",
    OperationId = "Project.Create",
    Tags = new[] { "ProjectEndpoints" })
  ]
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
