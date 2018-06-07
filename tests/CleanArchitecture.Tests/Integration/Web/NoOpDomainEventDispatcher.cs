using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Tests.Integration.Web
{
    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public void Dispatch(BaseDomainEvent domainEvent) { }
    }
}
