using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Commands.DeleteContributor;

public record DeleteContributorCommand(int ContributorId) : IRequest<Result>;
