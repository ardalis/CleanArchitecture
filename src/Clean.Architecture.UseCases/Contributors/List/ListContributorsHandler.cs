﻿using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Clean.Architecture.UseCases.Contributors.List;

public class ListContributorsHandler(IListContributorsQueryService _query)
  : IQueryHandler<ListContributorsQuery, Result<IEnumerable<ContributorDTO>>>
{
  public async Task<Result<IEnumerable<ContributorDTO>>> Handle(ListContributorsQuery request, CancellationToken cancellationToken)
  {
    IEnumerable<ContributorDTO> result = await _query.ListAsync();

    return Result.Success(result);
  }
}
