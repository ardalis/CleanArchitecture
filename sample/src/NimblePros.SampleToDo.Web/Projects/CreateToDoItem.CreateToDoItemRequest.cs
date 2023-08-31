using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NimblePros.SampleToDo.Web.Projects;

public class CreateToDoItemRequest
{
  public const string Route = "/Projects/{ProjectId:int}/ToDoItems";
  public static string BuildRoute(int projectId) => Route.Replace("{ProjectId:int}", projectId.ToString());

  [Required]
  [FromRoute]
  public int ProjectId { get; set; } = 0;

  [Required]
  public string Title { get; set; } = string.Empty;
  [Required]
  public string Description { get; set; } = string.Empty;

  public int? ContributorId { get; set; }
}
