using Ardalis.Result;
using Ardalis.SharedKernel;
using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.Create;

public class CreateProjectHandler : ICommandHandler<CreateProjectCommand, Result<int>>
{
  private readonly IRepository<Project> _repository;

  public CreateProjectHandler(IRepository<Project> repository)
  {
    _repository = repository;
  }

  public async Task<Result<int>> Handle(CreateProjectCommand request,
    CancellationToken cancellationToken)
  {
    var newProject = new Project(request.Name, Priority.Backlog);
    var createdItem = await _repository.AddAsync(newProject, cancellationToken);

    return createdItem.Id;
  }
}
