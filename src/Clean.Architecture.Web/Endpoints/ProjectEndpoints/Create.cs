using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{
    public class Create : BaseAsyncEndpoint
        .WithRequest<NewProjectRequest>
        .WithResponse<NewProjectResponse>
    {
        private readonly IRepository<Project> _repository;

        public Create(IRepository<Project> repository)
        {
            _repository = repository;
        }

        [HttpPost("/Projects")]
        [SwaggerOperation(
            Summary = "Creates a new ToDoItem",
            Description = "Creates a new ToDoItem",
            OperationId = "ToDoItem.Create",
            Tags = new[] { "ToDoItemEndpoints" })
        ]
        public override async Task<ActionResult<NewProjectResponse>> HandleAsync(NewProjectRequest request, CancellationToken cancellationToken)
        {
            var newProject = new Project(request.Name);

            var createdItem = await _repository.AddAsync(newProject);

            return Ok(createdItem);
        }
    }
}
