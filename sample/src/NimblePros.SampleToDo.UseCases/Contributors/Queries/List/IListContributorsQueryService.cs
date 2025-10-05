﻿namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.List;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListContributorsQueryService
{
  Task<PagedResult<ContributorDto>> ListAsync(int page, int perPage);
}
