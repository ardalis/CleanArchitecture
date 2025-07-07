using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects.GetWithAllItems;

public record GetProjectWithAllItemsQuery(ProjectId ProjectId) : IQuery<Result<ProjectWithAllItemsDTO>>;
