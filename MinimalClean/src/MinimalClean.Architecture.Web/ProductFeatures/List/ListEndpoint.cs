using FastEndpoints;
using FluentValidation;
using MinimalClean.Architecture.Web.ProductFeatures;

namespace MinimalClean.Architecture.Web.ProductFeatures.List;

public sealed class ListProductsRequest
{
  [BindFrom("page")]
  public int Page { get; init; } = 1;

  [BindFrom("per_page")]
  public int PerPage { get; init; } = Constants.DEFAULT_PAGE_SIZE;
}

public record ProductListResponse : PagedResult<ProductRecord>
{
  public ProductListResponse(IReadOnlyList<ProductRecord> Items, int Page, int PerPage, int TotalCount, int TotalPages)
    : base(Items, Page, PerPage, TotalCount, TotalPages)
  {
  }
}


public class ListEndpoint(IMediator mediator) : Endpoint<ListProductsRequest, ProductListResponse, ListProductsMapper>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Get("/Products");
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "List products with pagination";
      s.Description = "Retrieves a paginated list of all products. Supports GitHub-style pagination with 1-based page indexing and configurable page size.";
      s.ExampleRequest = new ListProductsRequest { Page = 1, PerPage = 10 };
      s.ResponseExamples[200] = new ProductListResponse(
        new List<ProductRecord>
        {
          new(1, "Sample Product 1", 9.99m),
          new(2, "Sample Product 2", 19.99m)
        },
        1, 10, 2, 1);

      s.Params["page"] = "1-based page index (default 1)";
      s.Params["per_page"] = $"Page size 1–{Constants.MAX_PAGE_SIZE} (default {Constants.DEFAULT_PAGE_SIZE})";

      s.Responses[200] = "Paginated list of products returned successfully";
      s.Responses[400] = "Invalid pagination parameters";
    });

    Tags("Products");

    Description(builder => builder
      .Accepts<ListProductsRequest>()
      .Produces<ProductListResponse>(200, "application/json")
      .ProducesProblem(400));
  }

  public override async Task HandleAsync(ListProductsRequest request, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new ListProductsQuery(request.Page, request.PerPage));
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

public sealed class ListProductsValidator : Validator<ListProductsRequest>
{
  public ListProductsValidator()
  {
    RuleFor(x => x.Page)
      .GreaterThanOrEqualTo(1)
      .WithMessage("page must be >= 1");

    RuleFor(x => x.PerPage)
      .InclusiveBetween(1, Constants.MAX_PAGE_SIZE)
      .WithMessage($"per_page must be between 1 and {Constants.MAX_PAGE_SIZE}");
  }
}

public sealed class ListProductsMapper
  : Mapper<ListProductsRequest, ProductListResponse, PagedResult<ProductDto>>
{
  public override ProductListResponse FromEntity(PagedResult<ProductDto> e)
  {
    var items = e.Items
      .Select(p => new ProductRecord(p.Id.Value, p.Name, p.UnitPrice))
      .ToList();

    return new ProductListResponse(items, e.Page, e.PerPage, e.TotalCount, e.TotalPages);
  }
}
