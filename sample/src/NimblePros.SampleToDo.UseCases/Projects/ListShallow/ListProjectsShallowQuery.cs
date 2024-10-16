namespace NimblePros.SampleToDo.UseCases.Projects.ListShallow;

public record ListProjectsShallowQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ProjectDTO>>>;
