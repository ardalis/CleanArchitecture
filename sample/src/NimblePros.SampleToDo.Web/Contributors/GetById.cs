using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.Get;

namespace NimblePros.SampleToDo.Web.Contributors;

/// <summary>Get a Contributor by integer ID.</summary>
/// <remarks>Takes a positive integer ID and returns a matching Contributor record.</remarks>
public class GetById(IMediator mediator)
  : Endpoint<GetContributorByIdRequest, Results<Ok<ContributorRecord>, NotFound>, GetContributorByIdMapper>
{
  public override void Configure()
  {
    Get(GetContributorByIdRequest.Route);
    AllowAnonymous();

    // Optional: document statuses for Swagger
    Summary(s =>
    {
      s.Summary = "Fetch a contributor by ID.";
      s.Response<ContributorRecord>(StatusCodes.Status200OK, "Found");
      s.Response(StatusCodes.Status404NotFound, "Not found");
    });
  }

  public override async Task<Results<Ok<ContributorRecord>, NotFound>>
    ExecuteAsync(GetContributorByIdRequest request, CancellationToken ct)
  {
    var result = await mediator.Send(new GetContributorQuery(ContributorId.From(request.ContributorId)), ct);

    return result.Status switch
    {
      ResultStatus.Ok => TypedResults.Ok(Map.FromEntity(result.Value)),
      ResultStatus.NotFound => TypedResults.NotFound(),
      _ => TypedResults.NotFound() // map other failures as needed
    };
  }
}
