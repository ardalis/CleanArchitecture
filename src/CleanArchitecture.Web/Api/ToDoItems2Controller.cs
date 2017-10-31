using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.Filters;
using CleanArchitecture.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class ToDoItems2Controller : Controller
    {
        private readonly IToDoAppService _appService;

        public ToDoItems2Controller(IToDoAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var items = _appService.List();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _appService.GetById(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDoItemDTO item)
        {
            var createdItem = _appService.CreateItem(item);
            return Ok(createdItem);
        }

        [HttpPost("{itemId}")]
        public IActionResult MarkComplete(int itemId)
        {
            _appService.MarkComplete(itemId);

            return Ok();
        }
    }
}
