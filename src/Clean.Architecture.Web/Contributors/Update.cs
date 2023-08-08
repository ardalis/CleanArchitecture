using Clean.Architecture.Core.ContributorAggregate;
using Ardalis.SharedKernel;
using FastEndpoints;
using Clean.Architecture.Web.Endpoints.ContributorEndpoints;
using Clean.Architecture.UseCases.Contributors.Create;
using Clean.Architecture.UseCases.Contributors.Update;
using MediatR;
using Ardalis.Result;

namespace Clean.Architecture.Web.ContributorEndpoints;

/// <summary>
/// Update an existing Contributor.
/// </summary>
/// <remarks>
/// Update an existing Contributor by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update : Endpoint<UpdateContributorRequest, UpdateContributorResponse>
{
  private readonly IRepository<Contributor> _repository;
  private readonly IMediator _mediator;

  public Update(IRepository<Contributor> repository, IMediator mediator)
  {
    _repository = repository;
    _mediator = mediator;
  }

  public override void Configure()
  {
    Put(UpdateContributorRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
    UpdateContributorRequest request,
    CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new UpdateContributorCommand(request.Id, request.Name!));

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    // TODO: Use Mediator
    var existingContributor = await _repository.GetByIdAsync(request.Id, cancellationToken);
    if (existingContributor == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      var dto = result.Value;
      Response = new UpdateContributorResponse(new ContributorRecord(dto.Id, dto.Name));
      return;
    }

  }
}
