using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Commands.CreateContributor;

public record CreateContributorCommand(string Name) : IRequest<Result<int>>;
