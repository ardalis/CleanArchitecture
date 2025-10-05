using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.Update;

public class UpdateProjectHandler : ICommandHandler<UpdateProjectCommand, Result<ProjectDto>>
{
  private readonly IRepository<Project> _repository;

  public UpdateProjectHandler(IRepository<Project> repository)
  {
    _repository = repository;
  }

  public async ValueTask<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
  {
    var existingEntity = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
    if (existingEntity == null)
    {
      return Result.NotFound();
    }

    existingEntity.UpdateName(request.NewName!);

    await _repository.UpdateAsync(existingEntity, cancellationToken);

    return Result.Success(new ProjectDto(existingEntity.Id.Value, existingEntity.Name.Value, existingEntity.Status.ToString()));
  }
}
