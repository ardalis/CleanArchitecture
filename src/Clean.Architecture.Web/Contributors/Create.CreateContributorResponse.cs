namespace Clean.Architecture.Web.Endpoints.ContributorEndpoints;

public class CreateContributorResponse
{
  public CreateContributorResponse(int id, string name)
  {
    Id = id;
    Name = name;
  }
  public int Id { get; set; }
  public string Name { get; set; }
}
