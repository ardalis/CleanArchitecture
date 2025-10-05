using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Delete;

namespace NimblePros.SampleToDo.Web.Contributors;

/// <summary>
/// Delete a Contributor.
/// </summary>
/// <remarks>
/// Delete a Contributor by providing a valid integer id.
/// </remarks>
public class Delete
  : Endpoint<DeleteContributorRequest, Results<NoContent, NotFound, ProblemHttpResult>>
{
  private readonly IMediator _mediator;
  public Delete(IMediator mediator) => _mediator = mediator;

  public override void Configure()
  {
    Delete(DeleteContributorRequest.Route);
    AllowAnonymous();
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeleteContributorRequest req, CancellationToken ct)
  {
    var cmd = new DeleteContributorCommand(ContributorId.From(req.ContributorId));
    var result = await _mediator.Send(cmd, ct);

    if (result.Status == ResultStatus.NotFound) return TypedResults.NotFound();

    if (result.IsSuccess) return TypedResults.NoContent();

    return TypedResults.Problem(
      title: "Delete failed",
      detail: string.Join("; ", result.Errors),
      statusCode: StatusCodes.Status400BadRequest);
  }
}
