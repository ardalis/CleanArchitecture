using Microsoft.AspNetCore.Diagnostics;

namespace NimblePros.SampleToDo.Web.Configurations;

public class GlobalExceptionHandler(
  IHostEnvironment env, 
  ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    logger.LogError(exception, "Unhandled exception occurred");

    var problemDetails = new
    {
      type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
      title = "Internal Server Error",
      status = StatusCodes.Status500InternalServerError,
      detail = env.IsDevelopment() ? exception.Message : "An internal server error occurred",
      instance = httpContext.Request.Path
    };

    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    httpContext.Response.ContentType = "application/json";
    
    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    return true;
  }
}
