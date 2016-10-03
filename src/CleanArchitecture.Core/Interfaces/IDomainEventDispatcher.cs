using CleanArchitecture.Core.Model;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
    }
}