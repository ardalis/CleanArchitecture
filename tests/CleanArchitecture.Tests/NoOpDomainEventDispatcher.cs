using System.Threading.Tasks;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.SharedKernel;

namespace Clean.Architecture.UnitTests
{
    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task Dispatch(BaseDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
