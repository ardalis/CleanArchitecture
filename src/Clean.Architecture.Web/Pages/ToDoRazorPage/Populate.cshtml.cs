using Clean.Architecture.Core;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.ToDoRazorPage
{
    public class PopulateModel : PageModel
    {
        private readonly IRepository<ToDoItem> _repository;

        public PopulateModel(IRepository<ToDoItem> repository)
        {
            _repository = repository;
        }

        public int RecordsAdded { get; set; }

        public void OnGet()
        {
            RecordsAdded = DatabasePopulator.PopulateDatabase(_repository);
        }
    }
}
