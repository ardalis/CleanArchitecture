using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.Commands;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Web.Handlers
{
    public class ListItemsHandler : IRequestHandler<ListItemsCommand, List<ToDoItemDTO>>
    {
        private readonly IRepository<ToDoItem> _todoRepository;

        public ListItemsHandler(IRepository<ToDoItem> todoRepository)
        {
            _todoRepository = todoRepository;
        }
        public List<ToDoItemDTO> Handle(ListItemsCommand message)
        {
            return _todoRepository.List()
                .Select(ToDoItemDTO.FromToDoItem)
                .ToList();
        }
    }
}
