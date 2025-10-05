using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects;
public record ProjectWithAllItemsDto(ProjectId Id, ProjectName Name, List<ToDoItemDto> Items, string Status);
