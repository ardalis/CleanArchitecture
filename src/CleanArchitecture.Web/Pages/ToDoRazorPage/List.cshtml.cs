using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace CleanArchitecture.Web.Pages.ToDoRazorPage
{
    public class ListModel : PageModel
    {
        private readonly IRepository<ToDoItem> _todoRepository;

        public List<ToDoItem> ToDoItems { get; set; }

        public ListModel(IRepository<ToDoItem> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public void OnGet()
        {
            ToDoItems = _todoRepository.List();
        }
    }
}
