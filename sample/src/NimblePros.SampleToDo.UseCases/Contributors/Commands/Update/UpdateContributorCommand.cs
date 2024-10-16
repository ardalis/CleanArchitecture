namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Update;

public record UpdateContributorCommand(int ContributorId, string NewName) : ICommand<Result<ContributorDTO>>;
