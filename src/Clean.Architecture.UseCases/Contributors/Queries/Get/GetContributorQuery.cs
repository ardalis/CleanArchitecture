using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Clean.Architecture.UseCases.Contributors.Queries.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
