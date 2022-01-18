using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class GetById : EndpointBaseAsync
    .WithRequest<GetProjectByIdRequest>
    .WithActionResult<GetProjectByIdResponse>
{
  private readonly IRepository<Project> _repository;

  public GetById(IRepository<Project> repository)
  {
    _repository = repository;
  }

  [HttpGet(GetProjectByIdRequest.Route)]
  [SwaggerOperation(
      Summary = "Gets a single Project",
      Description = "Gets a single Project by Id",
      OperationId = "Projects.GetById",
      Tags = new[] { "ProjectEndpoints" })
  ]
  public override async Task<ActionResult<GetProjectByIdResponse>> HandleAsync([FromRoute] GetProjectByIdRequest request,
      CancellationToken cancellationToken)
  {
    var spec = new ProjectByIdWithItemsSpec(request.ProjectId);
    var entity = await _repository.GetBySpecAsync(spec); // TODO: pass cancellation token
    if (entity == null) return NotFound();

    var response = new GetProjectByIdResponse
    (
        id: entity.Id,
        name: entity.Name,
        items: entity.Items.Select(item => new ToDoItemRecord(item.Id, item.Title, item.Description, item.IsDone)).ToList()
    );
    return Ok(response);
  }
}
