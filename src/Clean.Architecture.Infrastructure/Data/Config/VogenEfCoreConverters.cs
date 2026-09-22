using Clean.Architecture.Core.ContributorAggregate;
using Vogen;

namespace Clean.Architecture.Infrastructure.Data.Config;

// This type is intentionally used as a marker for the Vogen source generator.
#pragma warning disable CA1812, CA1852
[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
internal partial class VogenEfCoreConverters;
#pragma warning restore CA1812, CA1852
