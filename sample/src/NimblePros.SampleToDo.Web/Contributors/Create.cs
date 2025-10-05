using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;

namespace NimblePros.SampleToDo.Web.Contributors;

/// <summary>
/// Create a new Contributor
/// </summary>
/// <remarks>
/// Creates a new Contributor given a name.
/// </remarks>
public class Create(IMediator mediator)
  : Endpoint<CreateContributorRequest, Results<Created<CreateContributorResponse>, ValidationProblem, ProblemHttpResult>>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post(CreateContributorRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new CreateContributorRequest { Name = "Contributor Name" };
    });
  }

  public override async Task<Results<Created<CreateContributorResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateContributorRequest request, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new CreateContributorCommand(ContributorName.From(request.Name!)));

    return result.Status switch
    {
      ResultStatus.Ok =>
        TypedResults.Created($"/Contributors/{result.Value}", new CreateContributorResponse(result.Value, request.Name!)),
      ResultStatus.Invalid =>
        TypedResults.ValidationProblem(
          result.ValidationErrors
            .GroupBy(e => e.Identifier ?? string.Empty)
            .ToDictionary(
              g => g.Key,
              g => g.Select(e => e.ErrorMessage).ToArray()
            )
        ),
      _ =>
        TypedResults.Problem(
          title: "Create failed",
          detail: string.Join("; ", result.Errors),
          statusCode: StatusCodes.Status400BadRequest)
    };
  }
}
