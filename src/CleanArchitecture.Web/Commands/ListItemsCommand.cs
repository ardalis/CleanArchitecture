using CleanArchitecture.Web.ApiModels;
using MediatR;
using System.Collections.Generic;

namespace CleanArchitecture.Web.Commands
{
    public class ListItemsCommand : IRequest<List<ToDoItemDTO>>
    {
    }
}
