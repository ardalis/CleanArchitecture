using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Projects.Commands.Delete;

public record DeleteProjectCommand(int ProjectId) : ICommand<Result>;
