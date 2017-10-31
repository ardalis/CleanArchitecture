using MediatR;

namespace CleanArchitecture.Web.Commands
{
    public class MarkItemCompleteCommand : IRequest
    {
        public int Id { get; set; }
    }
}
