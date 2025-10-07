using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Delete;
using NimblePros.SampleToDo.Web.Extensions;

namespace NimblePros.SampleToDo.Web.Contributors;

public class Delete
  : Endpoint<DeleteContributorRequest,
             Results<NoContent,
                     NotFound,
                     ProblemHttpResult>>
{
  private readonly IMediator _mediator;
  public Delete(IMediator mediator) => _mediator = mediator;

  public override void Configure()
  {
    Delete(DeleteContributorRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Delete a contributor";
      s.Description = "Deletes an existing contributor by ID. This action cannot be undone.";
      s.ExampleRequest = new DeleteContributorRequest { ContributorId = 1 };
      
      // Document possible responses
      s.Responses[204] = "Contributor deleted successfully";
      s.Responses[404] = "Contributor not found";
      s.Responses[400] = "Invalid request or deletion failed";
    });
    
    // Add tags for API grouping
    Tags("Contributors");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<DeleteContributorRequest>()
      .Produces(204)
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeleteContributorRequest req, CancellationToken ct)
  {
    var cmd = new DeleteContributorCommand(ContributorId.From(req.ContributorId));
    var result = await _mediator.Send(cmd, ct);

    return result.ToDeleteResult();
  }
}
