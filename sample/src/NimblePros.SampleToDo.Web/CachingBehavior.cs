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

  private MemoryCacheEntryOptions _cacheOptions =>
          new MemoryCacheEntryOptions()
              .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cachingOptions.DefaultDurationSeconds));

  public async ValueTask<TResponse?> Handle(
      TRequest request,
      Mediator.MessageHandlerDelegate<TRequest, TResponse?> next,
      CancellationToken cancellationToken)
  {
    Guard.Against.Null(request, nameof(request));

    if (request is not ICacheable cacheable) return await next(request, cancellationToken);

    var cacheKey = cacheable.GetCacheKey();
    _logger.LogDebug("Checking cache for {CacheKey}", cacheKey);
    return await _cache.GetOrCreateAsync(cacheKey, async entry =>
    {
      _logger.LogInformation($"Cache miss. Getting data from database. ({cacheKey})");
      entry.SetOptions(_cacheOptions);
      return await next(request, cancellationToken);
    });
  }
}
