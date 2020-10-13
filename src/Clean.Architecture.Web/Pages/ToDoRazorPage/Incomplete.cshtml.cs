using System.Collections.Generic;
using System.Threading.Tasks;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.Core.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.ToDoRazorPage
{
    public class IncompleteModel : PageModel
    {
        private readonly IRepository _repository;

        public List<ToDoItem> ToDoItems { get; set; }

        public IncompleteModel(IRepository repository)
        {
            _repository = repository;
        }

        public async Task OnGetAsync()
        {
            var spec = new IncompleteItemsSpecification();

            ToDoItems = await _repository.ListAsync(spec);
        }
    }
}
