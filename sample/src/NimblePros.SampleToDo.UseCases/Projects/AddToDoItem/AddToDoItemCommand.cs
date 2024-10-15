using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Projects.AddToDoItem;

/// <summary>
/// Creates a new ToDoItem and adds it to a Project
/// </summary>
/// <param name="ProjectId"></param>
/// <param name="ContributorId"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
public record AddToDoItemCommand(int ProjectId, int? ContributorId, string Title, string Description) : ICommand<Result<int>>;
