namespace NimblePros.SampleToDo.Web.ProjectEndpoints;

public record ToDoItemRecord(int Id, string Title, string Description, bool IsDone, int? ContributorId);
