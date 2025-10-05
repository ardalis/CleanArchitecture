using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.List;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class FakeListContributorsQueryService : IListContributorsQueryService
{
  public Task<IEnumerable<ContributorDTO>> ListAsync()
  {
    var result = new List<ContributorDTO>() { 
      new ContributorDTO(ContributorId.From(1), ContributorName.From("Ardalis")),
      new ContributorDTO(ContributorId.From(2), ContributorName.From("Snowfrog"))
    };
    return Task.FromResult(result.AsEnumerable());
  }
}
