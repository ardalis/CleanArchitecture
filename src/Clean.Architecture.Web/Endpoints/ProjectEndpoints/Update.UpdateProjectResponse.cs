namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class UpdateProjectResponse
{
    public UpdateProjectResponse(ProjectRecord project)
    {
        Project = project;
    }
    public ProjectRecord Project { get; set; }
}
