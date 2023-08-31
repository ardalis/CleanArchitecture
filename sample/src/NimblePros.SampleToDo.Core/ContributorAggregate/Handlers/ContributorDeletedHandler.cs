using Ardalis.SharedKernel;
using NimblePros.SampleToDo.Core.ContributorAggregate.Events;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace NimblePros.SampleToDo.Core.ContributorAggregate.Handlers;

/// <summary>
/// NOTE: Internal because ContributorDeleted is also marked as internal.
/// </summary>
internal class ContributorDeletedHandler : INotificationHandler<ContributorDeletedEvent>
{
  private readonly IRepository<Project> _repository;
  private readonly ILogger<ContributorDeletedHandler> _logger;

  public ContributorDeletedHandler(IRepository<Project> repository,
    ILogger<ContributorDeletedHandler> logger)
  {
    _repository = repository;
    _logger = logger;
  }

  public async Task Handle(ContributorDeletedEvent domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Removing deleted contributor {contributorId} from all projects...", domainEvent.ContributorId);
    // Perform eventual consistency removal of contributors from projects when one is deleted
    var projectSpec = new ProjectsWithItemsByContributorIdSpec(domainEvent.ContributorId);
    var projects = await _repository.ListAsync(projectSpec, cancellationToken);
    foreach (var project in projects)
    {
      project.Items
        .Where(item => item.ContributorId == domainEvent.ContributorId)
        .ToList()
        .ForEach(item => item.RemoveContributor());
      await _repository.UpdateAsync(project, cancellationToken);
    }
  }
}
