using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            if (_todoRepository.List().Any()) return Ok(0);
            _todoRepository.Add(new ToDoItem()
            {
                Title = "One",
                Description = "The first item"
            });
            _todoRepository.Add(new ToDoItem()
            {
                Title = "Two",
                Description = "The second item"
            });
            _todoRepository.Add(new ToDoItem()
            {
                Title = "Three",
                Description = "The three item"
            });
            return Ok(3);
        }
    }
}
