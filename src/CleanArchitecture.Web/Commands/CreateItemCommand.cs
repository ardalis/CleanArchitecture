using CleanArchitecture.Web.ApiModels;
using MediatR;

namespace CleanArchitecture.Web.Commands
{
    public class CreateItemCommand : IRequest<ToDoItemDTO>
    {
        public ToDoItemDTO ItemToCreate { get; set; }
    }
}
