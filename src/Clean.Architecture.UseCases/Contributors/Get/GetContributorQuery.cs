using Clean.Architecture.Core.ContributorAggregate;

// CA1716 flags the namespace segment "Get" because it conflicts with a
// reserved keyword in some .NET languages. Renaming the namespace changes
// the public namespace and requires all references to be updated.
// The permanent fix is tracked separately in issue #1065.
#pragma warning disable CA1716 // Preserve the established public namespace.
namespace Clean.Architecture.UseCases.Contributors.Get;
#pragma warning restore CA1716

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
