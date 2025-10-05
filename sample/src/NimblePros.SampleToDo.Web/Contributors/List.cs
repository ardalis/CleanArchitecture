using FluentValidation;
using NimblePros.SampleToDo.UseCases.Contributors;
using NimblePros.SampleToDo.UseCases.Contributors.Queries.List;

namespace NimblePros.SampleToDo.Web.Contributors;

/// <summary>
/// List all Contributors
/// </summary>
/// <remarks>
/// List all contributors - returns a ContributorListResponse containing the Contributors.
/// NOTE: In DEV always returns a FAKE set of contributors
/// </remarks>
public class List(IMediator mediator) : Endpoint<ListContributorsRequest, ContributorListResponse, ListContributorsMapper>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Get("/Contributors");
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "List contributors (GitHub-style pagination).";
      s.Description = "Supports ?page=x&per_page=y with 1-based pages and capped page size.";
      s.Params["page"] = "1-based page index (default 1).";
      s.Params["per_page"] = $"Page size 1–{UseCases.Constants.MAX_PAGE_SIZE} (default {UseCases.Constants.DEFAULT_PAGE_SIZE}).";
      s.Response<ContributorListResponse>(StatusCodes.Status200OK, "Paged result.");
    });
  }

  public override async Task HandleAsync(ListContributorsRequest request, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new ListContributorsQuery(request.Page, request.PerPage));
    if (!result.IsSuccess)
    {
      await Send.ErrorsAsync(statusCode: 400, cancellationToken);
      return;
    }

    var pagedResult = result.Value;
    AddLinkHeader(pagedResult.Page, pagedResult.PerPage, pagedResult.TotalPages);

    var response = Map.FromEntity(pagedResult);
    await Send.OkAsync(response, cancellationToken);
  }

  private void AddLinkHeader(int page, int perPage, int totalPages)
  {
    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
    string Link(string rel, int p) => $"<{baseUrl}?page={p}&per_page={perPage}>; rel=\"{rel}\"";

    var parts = new List<string>();
    if (page > 1)
    {
      parts.Add(Link("first", 1));
      parts.Add(Link("prev", page - 1));
    }
    if (page < totalPages)
    {
      parts.Add(Link("next", page + 1));
      parts.Add(Link("last", totalPages));
    }

    if (parts.Count > 0)
      HttpContext.Response.Headers["Link"] = string.Join(", ", parts);
  }
}

public sealed class ListContributorsRequest
{
  // Bind to ?page=
  [BindFrom("page")]
  public int Page { get; init; } = 1;

  // Bind to ?per_page=
  [BindFrom("per_page")]
  public int PerPage { get; init; } = 30;
}

public sealed class ListContributorsValidator : Validator<ListContributorsRequest>
{
  public ListContributorsValidator()
  {
    RuleFor(x => x.Page)
      .GreaterThanOrEqualTo(1)
      .WithMessage("page must be >= 1");

    RuleFor(x => x.PerPage)
      .InclusiveBetween(1, UseCases.Constants.MAX_PAGE_SIZE)
      .WithMessage($"per_page must be between 1 and {UseCases.Constants.MAX_PAGE_SIZE}");
  }
}

public sealed class ListContributorsMapper
  : Mapper<ListContributorsRequest, ContributorListResponse, UseCases.PagedResult<ContributorDto>>
{
  public override ContributorListResponse FromEntity(UseCases.PagedResult<ContributorDto> e)
  {
    var items = e.Items
      .Select(c => new ContributorRecord(c.Id.Value, c.Name.Value))
      .ToList();

    return new ContributorListResponse(items, e.Page, e.PerPage, e.TotalCount, e.TotalPages);
  }
}




