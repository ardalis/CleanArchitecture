namespace NimblePros.SampleToDo.UseCases.Projects;
public record ProjectWithAllItemsDTO(int Id, string Name, List<ToDoItemDTO> Items, string Status);
