namespace Clean.Architecture.UseCases.Contributors.List;

public record ListContributorsQuery(int? Page = 1, int? PerPage = Constants.DefaultPageSize)
  : IQuery<Result<PagedResult<ContributorDto>>>;
