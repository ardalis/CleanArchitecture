namespace NimblePros.SampleToDo.UseCases.Projects.GetWithAllItems;

public record GetProjectWithAllItemsQuery(int ProjectId) : IQuery<Result<ProjectWithAllItemsDTO>>;
