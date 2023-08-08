
using NimblePros.SampleToDo.Web.ProjectEndpoints;

namespace NimblePros.SampleToDo.Web.Endpoints.ProjectEndpoints;

public class ProjectListResponse
{
  public List<ProjectRecord> Projects { get; set; } = new();
}
