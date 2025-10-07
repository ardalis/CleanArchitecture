namespace NimblePros.SampleToDo.Web.Projects;

public record GetProjectByIdResponse(int Id, string Name, List<ToDoItemRecord> Items);
