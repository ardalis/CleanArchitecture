using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Clean.Architecture.Core;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Web.ApiModels;

namespace Clean.Architecture.Web.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IRepository _repository;

        public ToDoController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var items = _repository.List<ToDoItem>()
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
