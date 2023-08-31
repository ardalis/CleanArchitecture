namespace NimblePros.SampleToDo.Web.Projects;

public class ListIncompleteItemsResponse
{
  public ListIncompleteItemsResponse(int projectId, List<ToDoItemRecord> incompleteItems)
  {
    ProjectId = projectId;
    IncompleteItems = incompleteItems;
  }
  public int ProjectId { get; set; }
  public List<ToDoItemRecord> IncompleteItems { get; set; }
}
