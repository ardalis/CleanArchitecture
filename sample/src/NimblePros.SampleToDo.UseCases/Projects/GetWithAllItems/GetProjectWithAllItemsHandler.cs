using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate.Specifications;

namespace NimblePros.SampleToDo.UseCases.Projects.GetWithAllItems;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetProjectWithAllItemsHandler : IQueryHandler<GetProjectWithAllItemsQuery, Result<ProjectWithAllItemsDto>>
{
  private readonly IReadRepository<Project> _repository;

  public GetProjectWithAllItemsHandler(IReadRepository<Project> repository)
  {
    _repository = repository;
  }

  public async ValueTask<Result<ProjectWithAllItemsDto>> Handle(GetProjectWithAllItemsQuery request, CancellationToken cancellationToken)
  {
    var spec = new ProjectByIdWithItemsSpec(request.ProjectId);
    var project = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (project == null) return Result.NotFound();

    var items = project.Items
              .Select(i => new ToDoItemDto(i.Id, i.Title, i.Description, i.IsDone, i.ContributorId)).ToList();
    return new ProjectWithAllItemsDto(project.Id, project.Name, items, project.Status.ToString())
      ;
  }
}
