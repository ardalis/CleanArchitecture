using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Delete;

public record DeleteContributorCommand(ContributorId ContributorId) : ICommand<Result>;
