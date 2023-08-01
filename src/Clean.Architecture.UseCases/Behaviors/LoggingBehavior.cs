using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clean.Architecture.UseCases.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  private readonly ILogger<Mediator> _logger;

  public LoggingBehavior(ILogger<Mediator> logger)
  {
    _logger = logger;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    Guard.Against.Null(request, nameof(request));
    if (_logger.IsEnabled(LogLevel.Information))
    {
      _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

      // Reflection! Could be a performance concern
      Type myType = request.GetType();
      IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
      foreach (PropertyInfo prop in props)
      {
        object? propValue = prop?.GetValue(request, null);
        _logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
      }
    }

    var sw = Stopwatch.StartNew();

    var response = await next();

    _logger.LogInformation("Handled {RequestName} with {Response} in {ms} ms", typeof(TRequest).Name, response, sw.ElapsedMilliseconds);
    sw.Stop();
    return response;
  }
}
