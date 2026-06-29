using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.UseCases.Contributors.GetContributor;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
