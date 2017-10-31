using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.Commands;
using CleanArchitecture.Web.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class ToDoItems3Controller : Controller
    {
        private readonly IMediator _mediator;

        public ToDoItems3Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var items = await _mediator.Send(new ListItemsCommand());
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var command = new GetItemCommand { Id = id };
            var item = await _mediator.Send(command);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDoItemDTO item)
        {
            var command = new CreateItemCommand { ItemToCreate = item };
            var createdItem = await _mediator.Send(command);
            return Ok(createdItem);
        }

        [HttpPost("{itemId}")]
        public async Task<IActionResult> MarkComplete(int itemId)
        {
            var command = new MarkItemCompleteCommand { Id = itemId };

            await _mediator.Send(command);

            return Ok();
        }

        [HttpPost("MarkComplete/{Id}")]
        public async Task<IActionResult> MarkComplete(MarkItemCompleteCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}
