namespace NimblePros.SampleToDo.UseCases.Projects.Commands.Update;

public record UpdateProjectCommand(int ProjectId, string NewName) : ICommand<Result<ProjectDTO>>;
