using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.Commands;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Web.Handlers
{

    public class GetItemHandler : IRequestHandler<GetItemCommand, ToDoItemDTO>
    {
        private readonly IRepository<ToDoItem> _todoRepository;

        public GetItemHandler(IRepository<ToDoItem> todoRepository)
        {
            _todoRepository = todoRepository;
        }
        public ToDoItemDTO Handle(GetItemCommand message)
        {
            return ToDoItemDTO.FromToDoItem(_todoRepository.GetById(message.Id));
        }
    }
}
