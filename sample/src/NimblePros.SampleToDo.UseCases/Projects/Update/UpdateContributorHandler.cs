using Ardalis.Result;
using Ardalis.SharedKernel;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects;
using NimblePros.SampleToDo.UseCases.Projects.Update;

namespace NimblePros.SampleToDo.UseCases.Contributors.Update;

public class UpdateProjectHandler : ICommandHandler<UpdateProjectCommand, Result<ProjectDTO>>
{
  private readonly IRepository<Project> _repository;

  public UpdateProjectHandler(IRepository<Project> repository)
  {
    _repository = repository;
  }

  public async Task<Result<ProjectDTO>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
  {
    var existingEntity = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
    if (existingEntity == null)
    {
      return Result.NotFound();
    }

    existingEntity.UpdateName(request.NewName!);

    await _repository.UpdateAsync(existingEntity, cancellationToken);

    return Result.Success(new ProjectDTO(existingEntity.Id, existingEntity.Name, existingEntity.Status.ToString()));
  }
}
