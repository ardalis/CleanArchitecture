using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ToDoItems
{
    public class GetById : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<ToDoItemResponse>
    {
        private readonly IRepository<ToDoItem> _repository;

        public GetById(IRepository<ToDoItem> repository)
        {
            _repository = repository;
        }

        [HttpGet("/ToDoItems/{id:int}")]
        [SwaggerOperation(
            Summary = "Gets a single ToDoItem",
            Description = "Gets a single ToDoItem by Id",
            OperationId = "ToDoItem.GetById",
            Tags = new[] { "ToDoItemEndpoints" })
        ]
        public override async Task<ActionResult<ToDoItemResponse>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(id);

            var response = new ToDoItemResponse
            {
                Id = item.Id,
                Description = item.Description,
                IsDone = item.IsDone,
                Title = item.Title
            };
            return Ok(response);
        }
    }
}
