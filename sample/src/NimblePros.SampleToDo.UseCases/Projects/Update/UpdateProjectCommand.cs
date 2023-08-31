using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Projects.Update;

public record UpdateProjectCommand(int ProjectId, string NewName) : ICommand<Result<ProjectDTO>>;
