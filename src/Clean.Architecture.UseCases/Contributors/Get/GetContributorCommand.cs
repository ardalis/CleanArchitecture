using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Contributors.Get;

public record GetContributorCommand(int ContributorId) : IRequest<Result<ContributorDTO>>;
