using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class CreateToDoItemRequest
{
  public const string Route = "/Projects/{ProjectId:int}/ToDoItems";
  public static string BuildRoute(int projectId) => Route.Replace("{ProjectId:int}", projectId.ToString());

  [Required]
  [FromRoute]
  public int ProjectId { get; set; }

  [Required]
  public string? Title { get; set; }
  public string? Description { get; set; }
  public int? ContributorId { get; set; }
}
