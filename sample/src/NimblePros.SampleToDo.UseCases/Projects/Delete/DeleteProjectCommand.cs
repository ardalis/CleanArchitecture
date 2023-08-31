using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Projects.Delete;

public record DeleteProjectCommand(int ProjectId) : ICommand<Result>;
