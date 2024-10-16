namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
