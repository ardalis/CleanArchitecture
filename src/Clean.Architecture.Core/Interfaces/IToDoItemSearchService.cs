using Ardalis.Result;
using Clean.Architecture.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Interfaces
{
    public interface IToDoItemSearchService
    {
        Task<Result<ToDoItem>> GetNextIncompleteItemAsync();
        Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(string searchString);
    }
}
