using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Clean.Architecture.Infrastructure.Data;

// Intercepts SaveChanges to dispatch domain events after changes are successfully saved
public class EventDispatchInterceptor(
  IDomainEventDispatcher domainEventDispatcher)
  : SaveChangesInterceptor
{
  private readonly IDomainEventDispatcher _domainEventDispatcher =
    domainEventDispatcher;

  // Called after SaveChangesAsync has completed successfully
  public override async ValueTask<int> SavedChangesAsync(
    SaveChangesCompletedEventData eventData,
    int result,
    CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(eventData);

    var context = eventData.Context;

    if (context is not AppDbContext appDbContext)
    {
      return await base
        .SavedChangesAsync(eventData, result, cancellationToken)
        .ConfigureAwait(false);
    }

    var entitiesWithEvents = appDbContext.ChangeTracker
      .Entries<HasDomainEventsBase>()
      .Select(entry => entry.Entity)
      .Where(entity => entity.DomainEvents.Count > 0)
      .ToArray();

    await _domainEventDispatcher
      .DispatchAndClearEvents(entitiesWithEvents)
      .ConfigureAwait(false);

    return await base
      .SavedChangesAsync(eventData, result, cancellationToken)
      .ConfigureAwait(false);
  }
}
