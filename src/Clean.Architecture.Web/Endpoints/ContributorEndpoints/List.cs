using FastEndpoints;
using MediatR;
using Clean.Architecture.UseCases.Queries.GetContributor;

namespace Clean.Architecture.Web.Endpoints.ContributorEndpoints;

public class List : EndpointWithoutRequest<ContributorListResponse>
{
  private readonly IMediator _mediator;

  public List(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Get("/Contributors");
    AllowAnonymous();
    Options(x => x
      .WithTags("ContributorEndpoints"));
  }
  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new ListContributorsCommand(null, null));

    Response = new ContributorListResponse { 
      Contributors = result.Value.Select(c => new ContributorRecord(c.Id, c.Name)).ToList()
    };
  }
}
