namespace NimblePros.SampleToDo.UseCases.Projects.Queries.GetWithAllItems;


public record GetProjectWithAllItemsQuery(int ProjectId) : IQuery<Result<ProjectWithAllItemsDTO>>;
