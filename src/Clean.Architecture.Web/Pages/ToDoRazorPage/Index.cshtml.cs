using System.Collections.Generic;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.ToDoRazorPage
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<ToDoItem> ToDoItems { get; set; }

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            ToDoItems = _repository.List<ToDoItem>();
        }
    }
}
