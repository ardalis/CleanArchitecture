using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record CreateContributorCommand(ContributorName Name) : Ardalis.SharedKernel.ICommand<Result<int>>;
