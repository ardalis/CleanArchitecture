namespace NimblePros.SampleToDo.UseCases.Projects.ListIncompleteItems;

public class ListIncompleteItemsByProjectHandler : IQueryHandler<ListIncompleteItemsByProjectQuery, Result<IEnumerable<ToDoItemDto>>>
{
  private readonly IListIncompleteItemsQueryService _query;

  public ListIncompleteItemsByProjectHandler(IListIncompleteItemsQueryService query)
  {
    _query = query;
  }

  public async ValueTask<Result<IEnumerable<ToDoItemDto>>> Handle(ListIncompleteItemsByProjectQuery request,
    CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync(request.ProjectId);

    return Result.Success(result);
  }
}
