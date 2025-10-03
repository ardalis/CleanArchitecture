﻿using Microsoft.EntityFrameworkCore.Diagnostics;
using NimblePros.SampleToDo.Core;

namespace NimblePros.SampleToDo.Infrastructure.Data;

// Intercepts SaveChanges to dispatch domain events after changes are successfully saved
public class EventDispatchInterceptor(IDomainEventDispatcher2 domainEventDispatcher) : SaveChangesInterceptor
{
  private readonly IDomainEventDispatcher2 _domainEventDispatcher = domainEventDispatcher;

  // Called after SaveChangesAsync has completed successfully
  public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
    CancellationToken cancellationToken = new CancellationToken())
  {
    var context = eventData.Context;
    if (context is not AppDbContext appDbContext)
    {
      return await base.SavedChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);
    }

    // Retrieve all tracked entities that have domain events
    var entitiesWithEvents = appDbContext.ChangeTracker.Entries<IHasDomainEvents>()
      .Select(e => e.Entity)
      .Where(e => e.DomainEvents.Any())
      .ToArray();

    // Dispatch and clear domain events
    await _domainEventDispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return await base.SavedChangesAsync(eventData, result, cancellationToken);

  }
}
