using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Contributors.Commands.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
