using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Commands.CreateContributor;

public record CreateContributorCommand(int ContributorId) : IRequest<Result>;
