using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.Commands.Create;

/// <summary>
/// Create a new Project.
/// </summary>
/// <param name="Name"></param>
public record CreateProjectCommand(ProjectName Name) : ICommand<Result<ProjectId>>;
