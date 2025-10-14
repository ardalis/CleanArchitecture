using Ardalis.GuardClauses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NimblePros.SampleToDo.UseCases;
using NimblePros.SampleToDo.Web.Configurations;

namespace NimblePros.SampleToDo.Web;

public class CachingBehavior<TRequest, TResponse>(IMemoryCache cache,
                                                  ILogger<CachingBehavior<TRequest, TResponse>> logger,
                                                  IOptions<CachingOptions> cachingOptions) :
    IPipelineBehavior<TRequest, TResponse?>
    where TRequest : notnull, IMessage
{
  private readonly IMemoryCache _cache = cache;
  private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger = logger;
  private readonly CachingOptions _cachingOptions = cachingOptions.Value;

  public async ValueTask<TResponse?> Handle(
      TRequest request,
      Mediator.MessageHandlerDelegate<TRequest, TResponse?> next,
      CancellationToken cancellationToken)
  {
    Guard.Against.Null(request, nameof(request));

    if (request is not ICacheable cacheable) return await next(request, cancellationToken);

    int cacheDurationSeconds = _cachingOptions.DefaultDurationSeconds;
    if (cacheable.CacheProfile != null)
    {
      var profile = _cachingOptions.Profiles.FirstOrDefault(p => p.Name.Equals(cacheable.CacheProfile, StringComparison.OrdinalIgnoreCase));
      if (profile != null)
      {
        _logger.LogInformation("Using cache profile {ProfileName} with duration {DurationSeconds} seconds.", profile.Name, profile.CacheDurationSeconds);
        cacheDurationSeconds = profile?.CacheDurationSeconds ?? _cachingOptions.DefaultDurationSeconds;
      }
    }
    var cacheOptions =
        new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheDurationSeconds));


  var cacheKey = cacheable.GetCacheKey();
    _logger.LogDebug("Checking cache for {CacheKey}", cacheKey);
    return await _cache.GetOrCreateAsync(cacheKey, async entry =>
    {
      _logger.LogInformation($"Cache miss. Getting data from database. ({cacheKey})");
      entry.SetOptions(cacheOptions);
      return await next(request, cancellationToken);
    });
  }
}
