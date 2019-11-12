using System.Threading.Tasks;
using Clean.Architecture.Core.SharedKernel;

namespace Clean.Architecture.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}