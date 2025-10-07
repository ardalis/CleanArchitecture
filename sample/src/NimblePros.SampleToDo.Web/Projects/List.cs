using FluentValidation;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.UseCases.Projects;
using NimblePros.SampleToDo.UseCases.Projects.ListShallow;

namespace NimblePros.SampleToDo.Web.Projects;

public class List(IMediator mediator) : Endpoint<ListProjectsRequest, ProjectListResponse, ListProjectsMapper>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Get($"/{nameof(Project)}s");
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "List projects with pagination";
      s.Description = "Retrieves a paginated list of all projects without their todo items. Supports GitHub-style pagination with 1-based page indexing and configurable page size.";
      s.ExampleRequest = new ListProjectsRequest { Page = 1, PerPage = 10 };
      s.ResponseExamples[200] = new ProjectListResponse(
        new List<ProjectRecord> 
        { 
          new(1, "Sample Project"), 
          new(2, "Another Project") 
        },
        1, 10, 2, 1);
      
      // Document pagination parameters
      s.Params["page"] = "1-based page index (default 1)";
      s.Params["per_page"] = $"Page size 1–{UseCases.Constants.MAX_PAGE_SIZE} (default {UseCases.Constants.DEFAULT_PAGE_SIZE})";
      
      // Document possible responses
      s.Responses[200] = "Paginated list of projects returned successfully";
      s.Responses[400] = "Invalid pagination parameters";
    });
    
    // Add tags for API grouping
    Tags("Projects");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<ListProjectsRequest>()
      .Produces<ProjectListResponse>(200, "application/json")
      .ProducesProblem(400));
  }

  public override async Task HandleAsync(ListProjectsRequest request, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new ListProjectsShallowQuery(null, null));

    if (!result.IsSuccess)
    {
      await Send.ErrorsAsync(statusCode: 400, cancellationToken);
      return;
    }

    // For now, simulate pagination behavior until ListProjectsShallowQuery supports it
    var allProjects = result.Value.ToList();
    var skip = (request.Page - 1) * request.PerPage;
    var pagedProjects = allProjects.Skip(skip).Take(request.PerPage).ToList();
    var totalCount = allProjects.Count;
    var totalPages = (int)Math.Ceiling((double)totalCount / request.PerPage);

    AddLinkHeader(request.Page, request.PerPage, totalPages);

    var pagedResult = new UseCases.PagedResult<ProjectDto>(
      pagedProjects, 
      request.Page, 
      request.PerPage, 
      totalCount, 
      totalPages);

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

public sealed class ListProjectsRequest
{
  // Bind to ?page=
  [BindFrom("page")]
  public int Page { get; init; } = 1;

  // Bind to ?per_page=
  [BindFrom("per_page")]
  public int PerPage { get; init; } = UseCases.Constants.DEFAULT_PAGE_SIZE;
}

public sealed class ListProjectsValidator : Validator<ListProjectsRequest>
{
  public ListProjectsValidator()
  {
    RuleFor(x => x.Page)
      .GreaterThanOrEqualTo(1)
      .WithMessage("page must be >= 1");

    RuleFor(x => x.PerPage)
      .InclusiveBetween(1, UseCases.Constants.MAX_PAGE_SIZE)
      .WithMessage($"per_page must be between 1 and {UseCases.Constants.MAX_PAGE_SIZE}");
  }
}

public sealed class ListProjectsMapper
  : Mapper<ListProjectsRequest, ProjectListResponse, UseCases.PagedResult<ProjectDto>>
{
  public override ProjectListResponse FromEntity(UseCases.PagedResult<ProjectDto> e)
  {
    var items = e.Items
      .Select(p => new ProjectRecord(p.Id, p.Name))
      .ToList();

    return new ProjectListResponse(items, e.Page, e.PerPage, e.TotalCount, e.TotalPages);
  }
}
