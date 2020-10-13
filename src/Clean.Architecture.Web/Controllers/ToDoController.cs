using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Core;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IRepository _repository;

        public ToDoController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var items = (await _repository.ListAsync<ToDoItem>())
                            .Select(ToDoItemDTO.FromToDoItem);
            return View(items);
        }

        public IActionResult Populate()
        {
            int recordsAdded = DatabasePopulator.PopulateDatabase(_repository);
            return Ok(recordsAdded);
        }
    }
}
