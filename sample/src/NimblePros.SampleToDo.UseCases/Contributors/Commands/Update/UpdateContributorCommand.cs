using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Update;

public record UpdateContributorCommand(int ContributorId, ContributorName NewName) : ICommand<Result<ContributorDTO>>;
