using Clean.Architecture.Core.SharedKernel;

namespace Clean.Architecture.Core.Interfaces
{
    public interface IHandle<T> where T : BaseDomainEvent
    {
        void Handle(T domainEvent);
    }
}