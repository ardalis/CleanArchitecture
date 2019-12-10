using System.Threading.Tasks;
using CleanArchitecture.SharedKernel;

namespace CleanArchitecture.SharedKernel.Interfaces
{
    public interface IHandle<in T> where T : BaseDomainEvent
    {
        Task Handle(T domainEvent);
    }
}