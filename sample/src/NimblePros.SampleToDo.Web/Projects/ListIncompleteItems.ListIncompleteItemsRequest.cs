using Microsoft.AspNetCore.Mvc;

namespace NimblePros.SampleToDo.Web.Endpoints.ProjectEndpoints;

public class ListIncompleteItemsRequest
{
  public const string Route = "/Projects/{ProjectId}/IncompleteItems";
  public static string BuildRoute(int projectId) => Route.Replace("{ProjectId:int}", projectId.ToString());


  [FromRoute]
  public int ProjectId { get; set; }
  //[FromQuery]
  //public string? SearchString { get; set; }
}
