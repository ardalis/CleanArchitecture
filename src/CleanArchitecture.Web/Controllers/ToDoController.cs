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
            return View();
        }
    }
}
