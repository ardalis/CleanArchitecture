namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class ListIncompleteResponse
{
  public ListIncompleteResponse(int projectId, List<ToDoItemRecord> incompleteItems)
  {
    ProjectId = projectId;
    IncompleteItems = incompleteItems;
  }
  public int ProjectId { get; set; }
  public List<ToDoItemRecord> IncompleteItems { get; set; }
}
