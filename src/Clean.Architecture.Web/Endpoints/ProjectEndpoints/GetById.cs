using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{
    public class GetById : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<ProjectResponse>
    {
        private readonly IRepository<Project> _repository;

        public GetById(IRepository<Project> repository)
        {
            _repository = repository;
        }

        [HttpGet("/Projects/{id:int}")]
        [SwaggerOperation(
            Summary = "Gets a single Project",
            Description = "Gets a single Project by Id",
            OperationId = "Projects.GetById",
            Tags = new[] { "ProjectEndpoints" })
        ]
        public override async Task<ActionResult<ProjectResponse>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(id);

            var spec = new ProjectByIdWithItemsSpec(id);
            var project = await _repository.GetBySpecAsync(spec);

            var response = new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Items = project.Items.Select(item => new ToDoItemRecord(item.Id, item.Title, item.Description, item.IsDone)).ToList()
            };
            return Ok(response);
        }
    }
}
