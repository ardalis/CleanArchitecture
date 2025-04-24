using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.Create;

public class CreateProjectHandler(IRepository<Project> repository) : ICommandHandler<CreateProjectCommand, Result<ProjectId>>
{
  private readonly IRepository<Project> _repository = repository;

  public async Task<Result<ProjectId>> Handle(CreateProjectCommand request,
    CancellationToken cancellationToken)
  {
    var newProject = new Project(ProjectName.From(request.Name));
    // NOTE: This implementation issues 3 queries due to Vogen implementation
    var createdItem = await _repository.AddAsync(newProject, cancellationToken);

    return createdItem.Id;
  }
}
