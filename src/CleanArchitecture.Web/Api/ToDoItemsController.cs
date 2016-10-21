using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    public class ToDoItemsController : Controller
    {
        private readonly IRepository<ToDoItem> _todoRepository;

        public ToDoItemsController(IRepository<ToDoItem> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public IActionResult List()
        {
            var items = _todoRepository.List();
            return Ok(items);
        }

        // GET: api/ToDoItems
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var items = _todoRepository.GetById(id);
            return Ok(items);
        }
    }
}
