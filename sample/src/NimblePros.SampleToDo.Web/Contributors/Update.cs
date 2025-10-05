using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Update;

namespace NimblePros.SampleToDo.Web.Contributors;

/// <summary>Update an existing Contributor.</summary>
/// <remarks>
/// Update by providing a fully defined replacement set of values.
/// </remarks>
public class Update(IMediator mediator)
  : Endpoint<
        UpdateContributorRequest,
        Results<Ok<UpdateContributorResponse>, NotFound, ProblemHttpResult>,
        UpdateContributorMapper>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Put(UpdateContributorRequest.Route);
    AllowAnonymous();

    // Optional but nice: enumerate for Swagger
    Summary(s =>
    {
      s.Summary = "Updates a contributor.";
      s.Description = "Returns 200 with updated resource, 404 if not found, 400 on business/validation errors.";
      s.Response<UpdateContributorResponse>(StatusCodes.Status200OK, "Updated");
      s.Response(StatusCodes.Status404NotFound, "Not found");
      s.Response<Microsoft.AspNetCore.Mvc.ProblemDetails>(StatusCodes.Status400BadRequest, "Problem");
    });
  }

  public override async Task<Results<Ok<UpdateContributorResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdateContributorRequest request, CancellationToken ct)
  {
    var cmd = new UpdateContributorCommand(
      ContributorId.From(request.Id),
      ContributorName.From(request.Name!));

    var result = await _mediator.Send(cmd, ct);

    if (result.Status == ResultStatus.NotFound) return TypedResults.NotFound();

    if (result.IsSuccess) return TypedResults.Ok(Map.FromEntity(result.Value));

    // Map remaining failures to RFC7807 Problem
    return TypedResults.Problem(
      title: "Update failed",
      detail: string.Join("; ", result.Errors),
      statusCode: StatusCodes.Status400BadRequest);
  }
}

public sealed class UpdateContributorMapper
  : Mapper<UpdateContributorRequest, UpdateContributorResponse, ContributorDTO>
{
  public override UpdateContributorResponse FromEntity(ContributorDTO e)
    => new(new ContributorRecord(e.Id.Value, e.Name.Value));
}
