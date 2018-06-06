using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Tests
{
    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public void Dispatch(BaseDomainEvent domainEvent) { }
    }
}
