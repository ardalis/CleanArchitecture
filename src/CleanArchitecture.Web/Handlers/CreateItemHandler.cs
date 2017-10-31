using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.Commands;
using MediatR;

namespace CleanArchitecture.Web.Handlers
{

    public class CreateItemHandler : IRequestHandler<CreateItemCommand, ToDoItemDTO>
    {
        private readonly IRepository<ToDoItem> _todoRepository;

        public CreateItemHandler(IRepository<ToDoItem> todoRepository)
        {
            _todoRepository = todoRepository;
        }
        public ToDoItemDTO Handle(CreateItemCommand message)
        {
            var itemToCreate = message.ItemToCreate;
            var todoItem = new ToDoItem()
            {
                Title = itemToCreate.Title,
                Description = itemToCreate.Description
            };
            _todoRepository.Add(todoItem);
            return ToDoItemDTO.FromToDoItem(todoItem);

        }
    }
}
