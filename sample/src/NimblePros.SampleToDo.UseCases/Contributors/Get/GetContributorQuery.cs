using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
