using CleanArchitecture.Core.Entities;
using CleanArchitecture.SharedKernel.Interfaces;
using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Api
{
    public class ToDoItemsController : BaseApiController
    {
        private readonly IRepository _repository;

        public ToDoItemsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var items = (await _repository.ListAsync<ToDoItem>())
                            .Select(ToDoItemDTO.FromToDoItem);
            return Ok(items);
        }

        // GET: api/ToDoItems
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item =  ToDoItemDTO.FromToDoItem(await _repository.GetByIdAsync<ToDoItem>(id));
            return Ok(item);
        }

        // POST: api/ToDoItems
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDoItemDTO item)
        {
            var todoItem = new ToDoItem()
            {
                Title = item.Title,
                Description = item.Description
            };
            await _repository.AddAsync(todoItem);
            return Ok(ToDoItemDTO.FromToDoItem(todoItem));
        }

        [HttpPatch("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var toDoItem = await _repository.GetByIdAsync<ToDoItem>(id);
            toDoItem.MarkComplete();
            await _repository.UpdateAsync(toDoItem);

            return Ok(ToDoItemDTO.FromToDoItem(toDoItem));
        }
    }
}
