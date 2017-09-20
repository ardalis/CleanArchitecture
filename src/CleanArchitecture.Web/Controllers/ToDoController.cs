using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CleanArchitecture.Web.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IRepository<ToDoItem> _todoRepository;

        public ToDoController(IRepository<ToDoItem> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public IActionResult Index()
        {
            var items = _todoRepository.List();
            return View(items);
        }

        public IActionResult Populate()
        {
            int recordsAdded = PopulateDatabase();
            return Ok(recordsAdded);
        }

        public int PopulateDatabase()
        {
            if (_todoRepository.List().Any()) return 0;
            _todoRepository.Add(new ToDoItem()
            {
                Title = "Get Sample Working",
                Description = "Try to get the sample to build."
            });
            _todoRepository.Add(new ToDoItem()
            {
                Title = "Review Solution",
                Description = "Review the different projects in the solution and how they relate to one another."
            });
            _todoRepository.Add(new ToDoItem()
            {
                Title = "Run and Review Tests",
                Description = "Make sure all the tests run and review what they are doing."
            });
            return _todoRepository.List().Count;
        }
    }
}
