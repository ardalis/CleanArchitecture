using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.SharedKernel;

namespace Clean.Architecture.Tests
{
    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public void Dispatch(BaseDomainEvent domainEvent) { }
    }
}
