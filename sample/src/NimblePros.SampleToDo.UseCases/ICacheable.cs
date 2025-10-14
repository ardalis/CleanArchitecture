namespace NimblePros.SampleToDo.UseCases;
public interface ICacheable
{
  string GetCacheKey();
  string? CacheProfile { get; } // TODO: Use SmartEnum for this with CacheProfile.None
}
