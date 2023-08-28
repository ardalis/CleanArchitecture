using System.ComponentModel.DataAnnotations;

namespace NimblePros.SampleToDo.Web.Contributors;

public class CreateContributorRequest
{
  public const string Route = "/Contributors";

  [Required]
  public string? Name { get; set; }
}
