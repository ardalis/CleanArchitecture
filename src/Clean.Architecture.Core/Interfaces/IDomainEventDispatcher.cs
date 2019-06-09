using Clean.Architecture.Core.SharedKernel;

namespace Clean.Architecture.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
    }
}