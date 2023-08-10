namespace NimblePros.SampleToDo.Web.Projects;

public class UpdateProjectResponse
{
  public UpdateProjectResponse(ProjectRecord project)
  {
    Project = project;
  }
  public ProjectRecord Project { get; set; }
}
