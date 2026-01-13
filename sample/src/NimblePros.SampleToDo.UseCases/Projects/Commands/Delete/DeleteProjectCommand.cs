using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.Commands.Delete;

public record DeleteProjectCommand(ProjectId ProjectId) : ICommand<Result>;
