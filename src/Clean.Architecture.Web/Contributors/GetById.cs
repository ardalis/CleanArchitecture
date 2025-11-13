using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.UseCases.Contributors;
using Clean.Architecture.UseCases.Contributors.Get;
using Clean.Architecture.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Clean.Architecture.Web.Contributors;

public class GetById(IMediator mediator)
  : Endpoint<GetContributorByIdRequest,
             Results<Ok<ContributorRecord>,
                     NotFound,
                     ProblemHttpResult>,
             GetContributorByIdMapper>
{
  public override void Configure()
  {
    Get(GetContributorByIdRequest.Route);
    AllowAnonymous();

    // Optional: document statuses for Swagger
    Summary(s =>
    {
      s.Summary = "Get a contributor by ID";
      s.Description = "Retrieves a specific contributor by their unique identifier. Returns detailed contributor information including ID and name.";
      s.ExampleRequest = new GetContributorByIdRequest { ContributorId = 1 };
      s.ResponseExamples[200] = new ContributorRecord(1, "John Doe", "+1 555-555-5555");

      // Document possible responses
      s.Responses[200] = "Contributor found and returned successfully";
      s.Responses[404] = "Contributor with specified ID not found";
    });

    // Add tags for API grouping
    Tags("Contributors");

    // Add additional metadata
    Description(builder => builder
      .Accepts<GetContributorByIdRequest>()
      .Produces<ContributorRecord>(200, "application/json")
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<ContributorRecord>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetContributorByIdRequest request, CancellationToken ct)
  {
    var result = await mediator.Send(new GetContributorQuery(ContributorId.From(request.ContributorId)), ct);

    return result.ToGetByIdResult(Map.FromEntity);
  }
}
public sealed class GetContributorByIdMapper
  : Mapper<GetContributorByIdRequest, ContributorRecord, ContributorDto>
{
  public override ContributorRecord FromEntity(ContributorDto e)
    => new(e.Id.Value, e.Name.Value, e.PhoneNumber.ToString());
}
