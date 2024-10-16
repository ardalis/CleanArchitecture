using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Clean.Architecture.UseCases.Contributors.Commands.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
