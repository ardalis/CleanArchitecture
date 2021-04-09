using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{
    public class Delete : BaseAsyncEndpoint
        .WithRequest<int>
        .WithoutResponse
    {
        private readonly IRepository<Project> _repository;

        public Delete(IRepository<Project> repository)
        {
            _repository = repository;
        }

        [HttpDelete("/Projects/{id:int}")]
        [SwaggerOperation(
            Summary = "Deletes a Project",
            Description = "Deletes a Project",
            OperationId = "Projects.Delete",
            Tags = new[] { "ProjectEndpoints" })
        ]
        public override async Task<ActionResult> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var itemToDelete = await _repository.GetByIdAsync(id);
            if (itemToDelete == null) return NotFound();

            await _repository.DeleteAsync(itemToDelete);

            return NoContent();
        }
    }
}
