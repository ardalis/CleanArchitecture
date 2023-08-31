namespace NimblePros.SampleToDo.Web.Projects;

public class GetProjectByIdResponse
{
  public GetProjectByIdResponse(int id, string name, List<ToDoItemRecord> items)
  {
    Id = id;
    Name = name;
    Items = items;
  }

  public int Id { get; set; }
  public string Name { get; set; }
  public List<ToDoItemRecord> Items { get; set; } = new();
}
