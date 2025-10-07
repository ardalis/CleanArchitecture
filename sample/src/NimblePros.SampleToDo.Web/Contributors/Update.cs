using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Update;
using NimblePros.SampleToDo.Web.Extensions;

namespace NimblePros.SampleToDo.Web.Contributors;

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
      s.Summary = "Update a contributor";
      s.Description = "Updates an existing contributor's information. The contributor name must be between 2 and 100 characters long.";
      s.ExampleRequest = new UpdateContributorRequest { Id = 1, Name = "Updated Name" };
      s.ResponseExamples[200] = new UpdateContributorResponse(new ContributorRecord(1, "Updated Name"));
      
      // Document possible responses
      s.Responses[200] = "Contributor updated successfully";
      s.Responses[404] = "Contributor with specified ID not found";
      s.Responses[400] = "Invalid input data or business rule violation";
    });
    
    // Add tags for API grouping
    Tags("Contributors");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<UpdateContributorRequest>("application/json")
      .Produces<UpdateContributorResponse>(200, "application/json")
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<Ok<UpdateContributorResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdateContributorRequest request, CancellationToken ct)
  {
    var cmd = new UpdateContributorCommand(
      ContributorId.From(request.Id),
      ContributorName.From(request.Name!));

    var result = await _mediator.Send(cmd, ct);

    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class UpdateContributorMapper
  : Mapper<UpdateContributorRequest, UpdateContributorResponse, ContributorDto>
{
  public override UpdateContributorResponse FromEntity(ContributorDto e)
    => new(new ContributorRecord(e.Id.Value, e.Name.Value));
}
