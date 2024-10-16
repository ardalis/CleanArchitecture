namespace NimblePros.SampleToDo.UseCases.Projects.Queries.ListShallow;

public record ListProjectsShallowQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ProjectDTO>>>;
