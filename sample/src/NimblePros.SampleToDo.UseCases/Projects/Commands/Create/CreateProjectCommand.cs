namespace NimblePros.SampleToDo.UseCases.Projects.Commands.Create;


/// <summary>
/// Create a new Project.
/// </summary>
/// <param name="Name"></param>
public record CreateProjectCommand(string Name) : Ardalis.SharedKernel.ICommand<Result<int>>;
