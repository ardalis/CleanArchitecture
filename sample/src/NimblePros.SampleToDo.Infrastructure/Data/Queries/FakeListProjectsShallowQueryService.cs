using NimblePros.SampleToDo.UseCases.Projects.ListShallow;
using NimblePros.SampleToDo.UseCases.Projects;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class FakeListProjectsShallowQueryService : IListProjectsShallowQueryService
{
  public async Task<IEnumerable<ProjectDTO>> ListAsync()
  {
    var testProject = new ProjectDTO(1000, "Test Project", "InProgress");
    return await Task.FromResult(new List<ProjectDTO> { testProject });
  }
}
