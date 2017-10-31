using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ApiModels;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Web.Services
{
    public class ToDoAppService : IToDoAppService
    {
        private readonly IRepository<ToDoItem> _todoRepository;

        public ToDoAppService(IRepository<ToDoItem> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public ToDoItemDTO CreateItem(ToDoItemDTO itemToCreate)
        {
            var todoItem = new ToDoItem()
            {
                Title = itemToCreate.Title,
                Description = itemToCreate.Description
            };
            _todoRepository.Add(todoItem);
            return ToDoItemDTO.FromToDoItem(todoItem);
        }

        public ToDoItemDTO GetById(int id)
        {
            return ToDoItemDTO.FromToDoItem(_todoRepository.GetById(id));
        }

        public IEnumerable<ToDoItemDTO> List()
        {
            return _todoRepository.List()
                            .Select(item => ToDoItemDTO.FromToDoItem(item));
        }

        public void MarkComplete(int itemId)
        {
            var item = _todoRepository.GetById(itemId);
            item.MarkComplete();
            _todoRepository.Update(item);
        }
    }
}
