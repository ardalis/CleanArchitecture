using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.Core.ProjectAggregate.Specifications;

namespace Clean.Architecture.Core.Services
{
    public class ToDoItemSearchService : IToDoItemSearchService
    {
        private readonly IRepository<Project> _repository;

        public ToDoItemSearchService(IRepository<Project> repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(int projectId, string searchString)
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

            var projectSpec = new ProjectByIdWithItemsSpec(projectId);
            var project = await _repository.GetBySpecAsync(projectSpec);

            var incompleteSpec = new IncompleteItemsSpec();

            try
            {
                //var items = await _repository.ListAsync(incompleteSpec);
                var items = incompleteSpec.Evaluate(project.Items).ToList();

                return new Result<List<ToDoItem>>(items);
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<List<ToDoItem>>.Error(new[] { ex.Message });
            }
        }

        public async Task<Result<ToDoItem>> GetNextIncompleteItemAsync(int projectId)
        {
            var projectSpec = new ProjectByIdWithItemsSpec(projectId);
            var project = await _repository.GetBySpecAsync(projectSpec);

            var incompleteSpec = new IncompleteItemsSpec();

            var items = incompleteSpec.Evaluate(project.Items).ToList();

            if (!items.Any())
            {
                return Result<ToDoItem>.NotFound();
            }

            return new Result<ToDoItem>(items.First());
        }
    }
}
