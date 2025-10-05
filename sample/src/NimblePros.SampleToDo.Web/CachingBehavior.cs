using Ardalis.GuardClauses;
using Microsoft.Extensions.Caching.Memory;

namespace NimblePros.SampleToDo.Web;

public class CachingBehavior<TRequest, TResponse> :
    Mediator.IPipelineBehavior<TRequest, TResponse?>
    where TRequest : notnull, Mediator.IMessage//, Mediator.IMessage<TResponse>
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<Mediator.Mediator> _logger;
    private MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(relative: TimeSpan.FromSeconds(10)); // TODO: Configure

    public CachingBehavior(IMemoryCache cache, ILogger<Mediator.Mediator> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async ValueTask<TResponse?> Handle(
        TRequest request,
        Mediator.MessageHandlerDelegate<TRequest, TResponse?> next,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var cacheKey = request.GetType().FullName ?? "";

        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            _logger.LogInformation($"Cache miss. Getting data from database. ({cacheKey})");
            entry.SetOptions(_cacheOptions);
            return await next(request, cancellationToken);
        });
    }
}
