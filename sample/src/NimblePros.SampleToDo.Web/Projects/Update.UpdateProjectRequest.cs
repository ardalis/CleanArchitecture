using System.ComponentModel.DataAnnotations;

namespace NimblePros.SampleToDo.Web.Projects;

public class UpdateProjectRequest
{
  public const string Route = "/Projects";
  public int Id { get; set; }
  public string? Name { get; set; }
}
