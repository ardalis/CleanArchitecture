using Microsoft.AspNetCore.Http.HttpResults;

namespace MinimalClean.Architecture.Web.Extensions;

public static class ResultExtensions
{
  /// <summary>
  /// Maps Result to TypedResults for endpoints that return Created, ValidationProblem, or ProblemHttpResult
  /// </summary>
  public static Results<Created<TResponse>, ValidationProblem, ProblemHttpResult> ToCreatedResult<TValue, TResponse>(
    this Result<TValue> result,
    Func<TValue, string> locationBuilder,
    Func<TValue, TResponse> mapResponse)
  {
    return result.Status switch
    {
      ResultStatus.Ok => TypedResults.Created(locationBuilder(result.Value), mapResponse(result.Value)),
      ResultStatus.Invalid => TypedResults.ValidationProblem(
        result.ValidationErrors
          .GroupBy(e => e.Identifier ?? string.Empty)
          .ToDictionary(
            g => g.Key,
            g => g.Select(e => e.ErrorMessage).ToArray()
          )
      ),
      _ => TypedResults.Problem(
        title: "Create failed",
        detail: string.Join("; ", result.Errors),
        statusCode: StatusCodes.Status400BadRequest)
    };
  }

  /// <summary>
  /// Maps Result to TypedResults for GetById endpoints that return Ok, NotFound, or ProblemHttpResult
  /// </summary>
  public static Results<Ok<TResponse>, NotFound, ProblemHttpResult> ToGetByIdResult<TValue, TResponse>(
    this Result<TValue> result,
    Func<TValue, TResponse> mapResponse) => ToOkOrNotFoundResult(result, mapResponse, "Get");

  /// <summary>
  /// Maps Result to TypedResults for Update endpoints that return Ok, NotFound, or ProblemHttpResult
  /// </summary>
  public static Results<Ok<TResponse>, NotFound, ProblemHttpResult> ToUpdateResult<TValue, TResponse>(
  this Result<TValue> result,
  Func<TValue, TResponse> mapResponse) => ToOkOrNotFoundResult(result, mapResponse, "Update");

  /// <summary>
  /// Maps Result to TypedResults for Delete endpoints that return NoContent, NotFound, or ProblemHttpResult
  /// </summary>
  public static Results<NoContent, NotFound, ProblemHttpResult> ToDeleteResult(
  this Result result) => result.Status switch
  {
    ResultStatus.Ok => TypedResults.NoContent(),
    ResultStatus.NotFound => TypedResults.NotFound(),
    _ => TypedResults.Problem(
        title: "Delete failed",
        detail: string.Join("; ", result.Errors),
        statusCode: StatusCodes.Status400BadRequest)
  };

  /// <summary>
  /// Private helper method for Ok/NotFound result patterns
  /// </summary>
  private static Results<Ok<TResponse>, NotFound, ProblemHttpResult> ToOkOrNotFoundResult<TValue, TResponse>(
  Result<TValue> result,
  Func<TValue, TResponse> mapResponse,
  string operationName) => result.Status switch
  {
    ResultStatus.Ok => TypedResults.Ok(mapResponse(result.Value)),
    ResultStatus.NotFound => TypedResults.NotFound(),
    _ => TypedResults.Problem(
        title: $"{operationName} failed",
        detail: string.Join("; ", result.Errors),
        statusCode: StatusCodes.Status400BadRequest)
  };

  /// <summary>
  /// Maps Result to TypedResults for endpoints that return Ok only (like List endpoints)
  /// </summary>
  public static Ok<TResponse> ToOkOnlyResult<TValue, TResponse>(
  this Result<TValue> result,
  Func<TValue, TResponse> mapResponse) => TypedResults.Ok(mapResponse(result.Value));
}
