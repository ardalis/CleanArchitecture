using System.Reflection;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace NimblePros.SampleToDo.Infrastructure.Data.Config;

internal class VogenIdValueGenerator<TContext, TEntityBase, TId> : ValueGenerator<TId>
    where TContext : DbContext
    where TEntityBase : EntityBase<TEntityBase, TId>
    where TId : IVogen<TId, int>
{
  private readonly PropertyInfo _matchPropertyGetter;

  public VogenIdValueGenerator()
  {
    var matchingProperties =
        typeof(TContext).GetProperties().Where(p => p.GetGetMethod().IsPublic && p.PropertyType == typeof(DbSet<TEntityBase>)).ToList();

    if (matchingProperties.Count == 0)
    {
      throw new InvalidOperationException($"No properties found in the EFCore context for a DBSet of {nameof(TEntityBase)}");
    }

    if (matchingProperties.Count > 1)
    {
      throw new InvalidOperationException($"Multiple properties found in the EFCore context for a DBSet of {nameof(TEntityBase)}");
    }

    _matchPropertyGetter = matchingProperties[0];
  }

  public override TId Next(EntityEntry entry)
  {
    TContext ctx = (TContext)entry.Context;

    DbSet<TEntityBase> entities = (DbSet<TEntityBase>)_matchPropertyGetter.GetValue(ctx);

    var next = Math.Max(
        MaxFrom(entities.Local),
        MaxFrom(entities)) + 1;

    return TId.From(next);

    static int MaxFrom(IEnumerable<TEntityBase> es) =>
        es.Any() ? es.Max(e => e.Id.Value) : 0;
  }


  public override bool GeneratesTemporaryValues => false;
}
