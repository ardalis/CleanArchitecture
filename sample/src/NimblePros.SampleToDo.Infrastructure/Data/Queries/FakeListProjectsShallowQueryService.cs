using NimblePros.SampleToDo.UseCases.Projects.ListShallow;
using NimblePros.SampleToDo.UseCases.Projects;

namespace NimblePros.SampleToDo.Infrastructure.Data.Queries;

public class FakeListProjectsShallowQueryService : IListProjectsShallowQueryService
{
  public async Task<IEnumerable<ProjectDto>> ListAsync()
  {
    var testProject = new ProjectDto(1000, "Test Project", "InProgress");
    return await Task.FromResult(new List<ProjectDto> { testProject });
  }
}
