namespace NimblePros.SampleToDo.Core.ContributorAggregate.Specifications;

public class ContributorByIdSpec : Specification<Contributor>, ISingleResultSpecification<Contributor>
{
  public ContributorByIdSpec(ContributorId contributorId)
  {
    Query
        .Where(contributor => contributor.Id == contributorId);
  }
}
