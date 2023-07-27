using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Queries.GetContributor;

public record GetContributorCommand(int ContributorId) : IRequest<Result<ContributorDTO>>;
