using System.ComponentModel.DataAnnotations;

namespace Clean.Architecture._1.Web.Endpoints.ProjectEndpoints;

public class CreateProjectRequest
{
  public const string Route = "/Projects";

  [Required]
  public string? Name { get; set; }
}
