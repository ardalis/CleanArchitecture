using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.Commands.Update;

public record UpdateProjectCommand(ProjectId ProjectId, ProjectName NewName) : ICommand<Result<ProjectDto>>;
