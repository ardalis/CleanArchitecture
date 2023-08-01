using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Contributors.List;

public record ListContributorsCommand(int? Skip, int? Take) : IRequest<Result<IEnumerable<ContributorDTO>>>;
