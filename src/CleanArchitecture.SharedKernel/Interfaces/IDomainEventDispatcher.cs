using System.Threading.Tasks;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.SharedKernel.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}