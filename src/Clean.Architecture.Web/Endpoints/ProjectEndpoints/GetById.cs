using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
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
            Tags = new[] { "Project" })
        ]
        public override async Task<ActionResult<ProjectResponse>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(id);

            var response = new ProjectResponse
            {
                Id = item.Id,
                Name = item.Name
            };
            return Ok(response);
        }
    }
}
