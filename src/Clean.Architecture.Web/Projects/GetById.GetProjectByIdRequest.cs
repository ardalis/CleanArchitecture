
namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class GetProjectByIdRequest
{
  public const string Route = "/Projects/{ProjectId:int}";
  public static string BuildRoute(int projectId) => Route.Replace("{ProjectId:int}", projectId.ToString());

  public int ProjectId { get; set; }
}
