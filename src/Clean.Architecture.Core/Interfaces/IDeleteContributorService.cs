using Ardalis.Result;

namespace Clean.Architecture.Core.Interfaces;

public interface IDeleteContributorService
{
    public Task<Result> DeleteContributor(int contributorId);
}
