using Ardalis.Result;
using NimblePros.SampleToDo.Core.Interfaces;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate.Specifications;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.Core.Services;

public class ToDoItemSearchService : IToDoItemSearchService
{
  private readonly IRepository<Project> _repository;

  public ToDoItemSearchService(IRepository<Project> repository)
  {
    _repository = repository;
  }

  public async Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(int projectId, string searchString)
  {
    if (string.IsNullOrEmpty(searchString))
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(searchString), ErrorMessage = $"{nameof(searchString)} is required." }
      };

      return Result<List<ToDoItem>>.Invalid(errors);
    }

    var projectSpec = new ProjectByIdWithItemsSpec(projectId);
    var project = await _repository.FirstOrDefaultAsync(projectSpec);

    // TODO: Optionally use Ardalis.GuardClauses Guard.Against.NotFound and catch
    if (project == null)
    {
      return Result<List<ToDoItem>>.NotFound();
    }

    var incompleteSpec = new IncompleteItemsSearchSpec(searchString);
    try
    {
      var items = incompleteSpec.Evaluate(project.Items).ToList();

      return new Result<List<ToDoItem>>(items);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<List<ToDoItem>>.Error( ex.Message );
    }
  }

  public async Task<Result<ToDoItem>> GetNextIncompleteItemAsync(int projectId)
  {
    var projectSpec = new ProjectByIdWithItemsSpec(projectId);
    var project = await _repository.FirstOrDefaultAsync(projectSpec);
    if (project == null)
    {
      return Result<ToDoItem>.NotFound();
    }

    var incompleteSpec = new IncompleteItemsSpec();
    var items = incompleteSpec.Evaluate(project.Items).ToList();
    if (!items.Any())
    {
      return Result<ToDoItem>.NotFound();
    }

    return new Result<ToDoItem>(items.First());
  }
}
