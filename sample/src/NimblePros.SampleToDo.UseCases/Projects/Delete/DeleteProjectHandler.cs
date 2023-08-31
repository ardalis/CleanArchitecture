using Ardalis.Result;
using Ardalis.SharedKernel;
using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.Delete;

public class DeleteProjectHandler : ICommandHandler<DeleteProjectCommand, Result>
{
  private readonly IRepository<Project> _repository;

  public DeleteProjectHandler(IRepository<Project> repository)
  {
    _repository = repository;
  }

  public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
  {
    var aggregateToDelete = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
    if (aggregateToDelete == null)
    {
      return Result.NotFound();
    }

    await _repository.DeleteAsync(aggregateToDelete, cancellationToken);

    return Result.Success();
  }
}
