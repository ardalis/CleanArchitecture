using System.Threading.Tasks;
using CleanArchitecture.SharedKernel;

namespace CleanArchitecture.SharedKernel.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}