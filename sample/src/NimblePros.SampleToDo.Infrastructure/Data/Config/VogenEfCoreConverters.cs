using NimblePros.SampleToDo.Core.ProjectAggregate;
using Vogen;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

[EfCoreConverter<ToDoItemId>]
[EfCoreConverter<ProjectName>]
[EfCoreConverter<ProjectId>]
internal partial class VogenEfCoreConverters;
