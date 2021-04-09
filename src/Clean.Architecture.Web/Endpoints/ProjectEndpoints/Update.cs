using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{
    public class Update : BaseAsyncEndpoint
        .WithRequest<UpdateProjectRequest>
        .WithResponse<UpdateProjectResponse>
    {
        private readonly IRepository<Project> _repository;

        public Update(IRepository<Project> repository)
        {
            _repository = repository;
        }

        [HttpPut("/Projects")]
        [SwaggerOperation(
            Summary = "Updates a Project",
            Description = "Updates a Project with a longer description",
            OperationId = "Projects.Update",
            Tags = new[] { "ProjectEndpoints" })
        ]
        public override async Task<ActionResult<UpdateProjectResponse>> HandleAsync(UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            var existingProject = await _repository.GetByIdAsync(request.Id);

            existingProject.UpdateName(request.Name);

            await _repository.UpdateAsync(existingProject);

            var response = new UpdateProjectResponse()
            {
                Project = new ProjectDTO(existingProject.Id, existingProject.Name)
            };
            return Ok(response);
        }
    }
}
