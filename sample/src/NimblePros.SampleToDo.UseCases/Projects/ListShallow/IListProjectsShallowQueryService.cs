namespace NimblePros.SampleToDo.UseCases.Projects.ListShallow;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListProjectsShallowQueryService
{
  Task<IEnumerable<ProjectDTO>> ListAsync();
}
