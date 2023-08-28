using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NimblePros.SampleToDo.Web.ProjectEndpoints;

public class MarkItemCompleteRequest
{
  public const string Route = "/Projects/{ProjectId:int}/ToDoItems/{ToDoItemId:int}";
  public static string BuildRoute(int projectId, int toDoItemId) => Route.Replace("{ProjectId:int}", projectId.ToString())
                                                                         .Replace("{ToDoItemId:int}", toDoItemId.ToString());

  [Required]
  [FromRoute]
  public int ProjectId { get; set; } = 0;
  [Required]
  [FromRoute]
  public int ToDoItemId { get; set; } = 0;

}
