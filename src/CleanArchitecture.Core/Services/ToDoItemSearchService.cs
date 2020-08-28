using Ardalis.Result;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Specifications;
using CleanArchitecture.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Services
{
    public class ToDoItemSearchService : IToDoItemSearchService
    {
        private readonly IRepository _repository;

        public ToDoItemSearchService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<ToDoItem>>> GetAllIncompleteItems(string searchString)
        {
            if(string.IsNullOrEmpty(searchString))
            {
                var errors = new List<ValidationError>();
                errors.Add(new ValidationError()
                {
                    Identifier = nameof(searchString),
                    ErrorMessage = $"{nameof(searchString)} is required."
                });
                return Result<List<ToDoItem>>.Invalid(errors);
            }

            var incompleteSpec = new IncompleteItemsSpecification();

            var items = await _repository.ListAsync(incompleteSpec);
            
            return new Result<List<ToDoItem>>(items);
        }

        public async Task<Result<ToDoItem>> GetNextIncompleteItem()
        {
            var incompleteSpec = new IncompleteItemsSpecification();

            var items = await _repository.ListAsync(incompleteSpec);

            if(!items.Any())
            {
                return Result<ToDoItem>.NotFound();
            }

            return new Result<ToDoItem>(items.First());
        }
    }
}
