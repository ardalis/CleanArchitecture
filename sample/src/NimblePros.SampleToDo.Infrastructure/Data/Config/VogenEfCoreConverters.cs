using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using Vogen;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ToDoItemId>]
[EfCoreConverter<ContributorName>]
[EfCoreConverter<ProjectName>]
[EfCoreConverter<ProjectId>]
[EfCoreConverter<ToDoItemTitle>]
[EfCoreConverter<ToDoItemDescription>]
internal partial class VogenEfCoreConverters;
