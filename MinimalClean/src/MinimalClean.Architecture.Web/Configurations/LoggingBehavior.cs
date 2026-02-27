using System.Diagnostics;

namespace Nimble.Modulith.Web;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IMessage
{
  private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    public async ValueTask<TResponse> Handle(
      TRequest request,
      MessageHandlerDelegate<TRequest, TResponse> next,
      CancellationToken cancellationToken)
  {
    if (_logger.IsEnabled(LogLevel.Information))
    {
      // Use structured logging with the whole object instead of reflection
      _logger.LogInformation("Handling {RequestName}: {@Request}", 
        typeof(TRequest).Name, request);
    }

    var sw = Stopwatch.StartNew();
    var response = await next(request, cancellationToken);

    _logger.LogInformation(
      "Handled {RequestName} with {Response} in {Ms} ms",
      typeof(TRequest).Name,
      response,
      sw.ElapsedMilliseconds);

    sw.Stop();
    return response;
  }
}
