using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ToDoItems
{
    public class Delete : BaseAsyncEndpoint
        .WithRequest<int>
        .WithoutResponse
    {
        private readonly IRepository<ToDoItem> _repository;

        public Delete(IRepository<ToDoItem> repository)
        {
            _repository = repository;
        }

        [HttpDelete("/ToDoItems/{id:int}")]
        [SwaggerOperation(
            Summary = "Deletes a ToDoItem",
            Description = "Deletes a ToDoItem",
            OperationId = "ToDoItem.Delete",
            Tags = new[] { "ToDoItemEndpoints" })
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
