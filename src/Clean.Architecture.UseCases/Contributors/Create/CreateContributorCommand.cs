using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Contributors.Create;

public record CreateContributorCommand(string Name) : IRequest<Result<int>>;
