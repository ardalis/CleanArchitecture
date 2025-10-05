using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects;
public record ProjectWithAllItemsDTO(ProjectId Id, ProjectName Name, List<ToDoItemDTO> Items, string Status);
