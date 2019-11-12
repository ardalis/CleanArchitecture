using System.Threading.Tasks;
using Clean.Architecture.Core.SharedKernel;

namespace Clean.Architecture.Core.Interfaces
{
    public interface IHandle<in T> where T : BaseDomainEvent
    {
        Task Handle(T domainEvent);
    }
}