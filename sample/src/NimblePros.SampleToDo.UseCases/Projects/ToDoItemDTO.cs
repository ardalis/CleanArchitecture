using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.UseCases.Projects;

public record ToDoItemDTO(ToDoItemId Id, string Title, string Description, bool IsComplete, ContributorId? ContributorId);
