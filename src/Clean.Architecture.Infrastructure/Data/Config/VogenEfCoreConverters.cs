using Clean.Architecture.Core.ContributorAggregate;
using Vogen;

namespace Clean.Architecture.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
internal partial class VogenEfCoreConverters;
