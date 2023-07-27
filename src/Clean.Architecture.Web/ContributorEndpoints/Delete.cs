using FastEndpoints;
using Ardalis.Result;
using Clean.Architecture.UseCases.Commands.DeleteContributor;
using MediatR;
using Clean.Architecture.Web.Endpoints.ContributorEndpoints;

namespace Clean.Architecture.Web.ContributorEndpoints;

public class Delete : Endpoint<DeleteContributorRequest>
{
  private readonly IMediator _mediator;

  public Delete(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Delete(DeleteContributorRequest.Route);
    AllowAnonymous();
    Options(x => x
      .WithTags("ContributorEndpoints"));
  }
  public override async Task HandleAsync(
    DeleteContributorRequest request,
    CancellationToken cancellationToken)
  {
    var command = new DeleteContributorCommand(request.ContributorId);

    var result = await _mediator.Send(command);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    // TODO: Handle other issues

    if (result.IsSuccess)
    {
      await SendNoContentAsync(cancellationToken);
    };

  }
}
