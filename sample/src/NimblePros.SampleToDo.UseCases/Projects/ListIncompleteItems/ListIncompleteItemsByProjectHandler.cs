using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;

public class ListIncompleteItemsByProjectHandler : IQueryHandler<ListIncompleteItemsByProjectQuery, Result<IEnumerable<ToDoItemDTO>>>
{
  private readonly IListIncompleteItemsQueryService _query;

  public ListIncompleteItemsByProjectHandler(IListIncompleteItemsQueryService query)
  {
    _query = query;
  }

  public async Task<Result<IEnumerable<ToDoItemDTO>>> Handle(ListIncompleteItemsByProjectQuery request,
    CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync(request.ProjectId);

    return Result.Success(result);
  }
}
