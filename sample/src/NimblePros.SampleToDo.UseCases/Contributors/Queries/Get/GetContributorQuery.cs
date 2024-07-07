using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Contributors.Queries.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
