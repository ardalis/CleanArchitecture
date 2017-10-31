using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.Commands;
using MediatR;

namespace CleanArchitecture.Web.Handlers
{
    public class MarkCompleteHandler : IRequestHandler<MarkItemCompleteCommand>
    {
        private readonly IRepository<ToDoItem> _todoRepository;

        public MarkCompleteHandler(IRepository<ToDoItem> todoRepository)
        {
            _todoRepository = todoRepository;
        }
        public void Handle(MarkItemCompleteCommand message)
        {
            var item = _todoRepository.GetById(message.Id);
            item.MarkComplete();
            _todoRepository.Update(item);
        }
    }
}
