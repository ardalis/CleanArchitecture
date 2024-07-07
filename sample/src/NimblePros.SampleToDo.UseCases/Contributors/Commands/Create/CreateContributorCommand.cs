using Ardalis.Result;

namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record CreateContributorCommand(string Name) : Ardalis.SharedKernel.ICommand<Result<int>>;
