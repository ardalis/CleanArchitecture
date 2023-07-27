using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Queries.GetContributor;

public record ListContributorsCommand(int? Skip, int? Take) : IRequest<Result<IEnumerable<ContributorDTO>>>;
