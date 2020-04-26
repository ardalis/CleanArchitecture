using Ardalis.ApiEndpoints;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Endpoints.ToDoItems
{
    public class GetById : BaseAsyncEndpoint<int,ToDoItemResponse>
    {
        private readonly IRepository _repository;

        public GetById(IRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("/ToDoItems/{id:int}")]
        [SwaggerOperation(Tags = new[] { "ToDoItem" })]
        public override async Task<ActionResult<ToDoItemResponse>> HandleAsync(int id)
        {
            var item = await _repository.GetByIdAsync<ToDoItem>(id);

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
