using CleanArchitecture.Core.Entities;
using CleanArchitecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Endpoints.ToDoItems
{
    public class CreateEndpoint : BaseEndpoint<CreateCommand, CreatedResult>
    {
        private readonly IRepository _repository;

        public CreateEndpoint(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("/endpoints/items")]
        public override ActionResult<CreatedResult> Handle([FromBody]CreateCommand request)
        {
            var todoItem = new ToDoItem()
            {
                Title = request.Title,
                Description = request.Description
            };
            _repository.Add(todoItem);

            var result = new CreatedResult
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description
            };
            return Ok(result);
        }
    }

}
