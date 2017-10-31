using CleanArchitecture.Web.ApiModels;
using MediatR;

namespace CleanArchitecture.Web.Commands
{
    public class GetItemCommand : IRequest<ToDoItemDTO>
    {
        public int Id { get; set; }
    }
}
