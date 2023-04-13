using Ardalis.Result;

namespace Clean.Architecture.Core.Interfaces;

public interface IDeleteContributorService
{
  // This service and method exist to provide a place in which to fire domain events
  // when deleting this aggregate root entity
  public Task<Result> DeleteContributor(int contributorId);
}
