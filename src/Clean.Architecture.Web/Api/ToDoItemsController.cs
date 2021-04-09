using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Api
{
    public class ToDoItemsController : BaseApiController
    {
        private readonly IRepository<Project> _repository;

        public ToDoItemsController(IRepository<Project> repository)
        {
            _repository = repository;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var projectSpec = new ProjectByIdWithItemsSpec(1); // TODO: Get from route
            var project = await _repository.GetBySpecAsync(projectSpec);

            var items = project.Items
                            .Select(ToDoItemDTO.FromToDoItem);
            return Ok(items);
        }

        // GET: api/ToDoItems
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var projectSpec = new ProjectByIdWithItemsSpec(1);
            var project = await _repository.GetBySpecAsync(projectSpec);

            var item =  ToDoItemDTO.FromToDoItem(project.Items.FirstOrDefault(i => i.Id == id));
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
            var projectSpec = new ProjectByIdWithItemsSpec(1);
            var project = await _repository.GetBySpecAsync(projectSpec);

            project.AddItem(todoItem);
            await _repository.UpdateAsync(project);

            return Ok(ToDoItemDTO.FromToDoItem(todoItem));
        }

        [HttpPatch("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var projectSpec = new ProjectByIdWithItemsSpec(1);
            var project = await _repository.GetBySpecAsync(projectSpec);

            var toDoItem = project.Items.First(item => item.Id == id);
            toDoItem.MarkComplete();
            await _repository.UpdateAsync(project);

            return Ok(ToDoItemDTO.FromToDoItem(toDoItem));
        }
    }
}
