namespace NimblePros.SampleToDo.UseCases.Projects.Queries.ListShallow;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListProjectsShallowQueryService
{
  Task<IEnumerable<ProjectDto>> ListAsync();
}
