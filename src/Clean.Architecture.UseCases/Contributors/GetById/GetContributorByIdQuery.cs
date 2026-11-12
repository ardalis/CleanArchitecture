using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.UseCases.Contributors.GetById;

public record GetContributorByIdQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
