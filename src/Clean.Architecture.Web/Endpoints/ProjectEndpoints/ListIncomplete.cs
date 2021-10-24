using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Clean.Architecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class ListIncomplete : BaseAsyncEndpoint
    .WithRequest<ListIncompleteRequest>
    .WithResponse<ListIncompleteResponse>
{
    private readonly IToDoItemSearchService _searchService;

    public ListIncomplete(IToDoItemSearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("/Projects/{ProjectId}/IncompleteItems")]
    [SwaggerOperation(
        Summary = "Gets a list of a project's incomplete items",
        Description = "Gets a list of a project's incomplete items",
        OperationId = "Project.ListIncomplete",
        Tags = new[] { "ProjectEndpoints" })
    ]
    public override async Task<ActionResult<ListIncompleteResponse>> HandleAsync([FromQuery] ListIncompleteRequest request, CancellationToken cancellationToken)
    {
        var response = new ListIncompleteResponse();
        var result = await _searchService.GetAllIncompleteItemsAsync(request.ProjectId, request.SearchString);

        if (result.Status == Ardalis.Result.ResultStatus.Ok)
        {
            response.ProjectId = request.ProjectId;
            response.IncompleteItems = new List<ToDoItemRecord>(
                    result.Value.Select(
                        item => new ToDoItemRecord(item.Id,
                        item.Title,
                        item.Description,
                        item.IsDone)));
        }
        else if (result.Status == Ardalis.Result.ResultStatus.Invalid)
        {
            return BadRequest(result.ValidationErrors);
        }
        else if (result.Status == Ardalis.Result.ResultStatus.NotFound)
        {
            return NotFound();
        }

        return Ok(response);
    }
}
