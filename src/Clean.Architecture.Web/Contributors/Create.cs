using Clean.Architecture.UseCases.Contributors.Create;
using FastEndpoints;
using MediatR;

namespace Clean.Architecture.Web.Contributors;

/// <summary>
/// Create a new Contributor
/// </summary>
/// <remarks>
/// Creates a new Contributor given a name.
/// </remarks>
public class Create(IMediator _mediator)
  : Endpoint<CreateContributorRequest, CreateContributorResponse>
{
  public override void Configure()
  {
    Post(CreateContributorRequest.Route);
    AllowAnonymous();

    // XML Docs are used by default but are overridden by these properties:
    //s.Summary = "Create a new Contributor.";
    //s.Description = "Create a new Contributor. A valid name is required.";
    Summary(s =>
      s.ExampleRequest = new CreateContributorRequest { Name = "Contributor Name" });
  }

  public override async Task HandleAsync(
    CreateContributorRequest request,
    CancellationToken cancellationToken)
  {
    Ardalis.Result.Result<int> result = await _mediator.Send(new CreateContributorCommand(request.Name!), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CreateContributorResponse(result.Value, request.Name!);
      return;
    }
    // TODO: Handle other cases as necessary
  }
}
