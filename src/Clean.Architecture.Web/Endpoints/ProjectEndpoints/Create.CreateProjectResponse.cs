namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class CreateProjectResponse
{
    public CreateProjectResponse(int id, string name)
    {
        Id = Id;
        Name = name;
    }
    public int Id { get; set; }
    public string Name { get; set; }
}
