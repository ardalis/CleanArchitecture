using NimblePros.SampleToDo.Core.ContributorAggregate;

namespace NimblePros.SampleToDo.Core.ProjectAggregate.Specifications;

public class ProjectsWithItemsByContributorIdSpec : Specification<Project>
{
  public ProjectsWithItemsByContributorIdSpec(ContributorId contributorId)
  {
    Query
        .Where(project => project.Items.Any(item => item.ContributorId == contributorId))
        .Include(project => project.Items);
  }
}
