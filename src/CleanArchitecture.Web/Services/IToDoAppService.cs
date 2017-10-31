using CleanArchitecture.Web.ApiModels;
using System.Collections.Generic;

namespace CleanArchitecture.Web.Services
{
    public interface IToDoAppService
    {
        ToDoItemDTO GetById(int id);
        ToDoItemDTO CreateItem(ToDoItemDTO itemToCreate);
        IEnumerable<ToDoItemDTO> List();
        void MarkComplete(int itemId);
    }
}
