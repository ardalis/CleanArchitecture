using Clean.Architecture.Core.ContributorAggregate;
using Ardalis.SharedKernel;
using FastEndpoints;
using Clean.Architecture.Web.Endpoints.ContributorEndpoints;

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

  public Update(IRepository<Contributor> repository)
  {
    _repository = repository;
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
    // TODO: Use Mediator
    var existingContributor = await _repository.GetByIdAsync(request.Id, cancellationToken);
    if (existingContributor == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    existingContributor.UpdateName(request.Name!);

    await _repository.UpdateAsync(existingContributor, cancellationToken);

    var response = new UpdateContributorResponse(
        contributor: new ContributorRecord(existingContributor.Id, existingContributor.Name)
    );

    await SendAsync(response, cancellation: cancellationToken);
  }
}
