using Ardalis.Result;
using CleanArchitecture.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IToDoItemSearchService
    {
        Task<Result<ToDoItem>> GetNextIncompleteItem();
        Task<Result<List<ToDoItem>>> GetAllIncompleteItems(string searchString);
    }
}
