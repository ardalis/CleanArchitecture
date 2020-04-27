using Ardalis.ApiEndpoints;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Endpoints.ToDoItems
{
    public class Update : BaseAsyncEndpoint<UpdateToDoItemRequest,ToDoItemResponse>
    {
        private readonly IRepository _repository;

        public Update(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPut("/ToDoItems")]
        [SwaggerOperation(
            Summary = "Updates a ToDoItem",
            Description = "Updates a ToDoItem",
            OperationId = "ToDoItem.Update",
            Tags = new[] { "ToDoItemEndpoint" })
        ]
        public override async Task<ActionResult<ToDoItemResponse>> HandleAsync(UpdateToDoItemRequest request)
        {
            var existingItem = await _repository.GetByIdAsync<ToDoItem>(request.Id);

            existingItem.Title = request.Title;
            existingItem.Description = request.Description;

            await _repository.UpdateAsync(existingItem);

            var response = new ToDoItemResponse
            {
                Id = existingItem.Id,
                Title = existingItem.Title,
                Description = existingItem.Description,
                IsDone = existingItem.IsDone
            };
            return Ok(response);
        }
    }
}
